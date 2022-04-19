using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.ExceptionHandling;
using BuildingBlocks.Infrastructure.HealthChecks;
using Carts.Api;
using Catalog.Api;
using FluentValidation;
using Grpc.Net.ClientFactory;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shopping.Api;
using Shopping.Infrastructure.Behaviors;
using Shopping.Infrastructure.Consumers;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System;
using BuildingBlocks.Infrastructure.AccountContext;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Marten;
using Media;
using Search.Api;
using Shopping.Application.Data;
using Shopping.Application.Services;
using Shopping.Infrastructure.DataSources;
using Shopping.Infrastructure.Gateways;
using Weasel.Postgresql;

// Enable W3C Trace Context support for distributed tracing
Activity.DefaultIdFormat = ActivityIdFormat.W3C;

// Allow grpc to operate in a non-TLS environment
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

// Build
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.LogToConsole();

// Logging
builder.Services.AddLogging(c => {
    c.AddSimpleConsole(opt=>{
        opt.SingleLine = true;
        opt.IncludeScopes = true;
    });
});

// Application
var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddMediatR(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddScoped<ICartsServiceGateway, CartsServiceGateway>();
builder.Services.AddScoped<ICatalogServiceGateway, CatalogServiceGateway>();
builder.Services.AddScoped<ISearchServiceGateway, SearchServiceGateway>();
builder.Services.AddScoped<IMediaServiceGateway, MediaServiceGateway>();

// Infrastructure
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// MultiAccount
builder.Services.AddScopedAccountContext();
builder.Services.AddAccountContextAwareServerInterceptor();
builder.Services.AddAccountContextAwareClientInterceptor();

// Bus
builder.Services.AddMassTransit(x => {
    x.AddConsumer<AccountAddedConsumer>();
    x.AddConsumer<StoreAddedConsumer>();
    x.AddConsumer<StoreUpdatedConsumer>();
    x.AddConsumer<ProductAddedConsumer>();
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration.GetRabbitUri());
        cfg.ConfigureEndpoints(context);
        cfg.ReceiveEndpoint("backend_shopping", e => {
            e.ConfigureConsumer<AccountAddedConsumer>(context);
            e.ConfigureConsumer<StoreAddedConsumer>(context);
            e.ConfigureConsumer<StoreUpdatedConsumer>(context);
            e.ConfigureConsumer<ProductAddedConsumer>(context);
        });
    });
});
builder.Services.AddMassTransitHostedService();

// Persistence
builder.Services.AddScoped<IDataReader, MartenDataReader>();
builder.Services.AddScoped<IDataWriter, MartenDataWriter>();
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetPostgresConnectionString());
    options.DatabaseSchemaName = builder.Configuration.GetPostgresSchema();
    options.AutoCreateSchemaObjects = AutoCreate.All;
    options.Schema.For<AccountData>().Identity(x => x.AccountId).DocumentAlias("accounts");
    options.Schema.For<StoreData>().Identity(x => x.StoreId).DocumentAlias("stores");
    options.Schema.For<ProductData>().Identity(x => x.ProductId).DocumentAlias("products");
});

// GRPC Server
builder.Services.AddGrpc(c => {
    c.Interceptors.Add<GrpcExceptionInterceptor>();
    c.Interceptors.Add<AccountContextAwareServerInterceptor>();
    c.EnableDetailedErrors = true;
});

// GRPC Client
builder.Services
    .AddGrpcClient<CatalogApi.CatalogApiClient>(o => { o.Address = builder.Configuration.GetCatalogServiceGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client)
    .EnableCallContextPropagation();

builder.Services
    .AddGrpcClient<CartsApi.CartsApiClient>(o => { o.Address = builder.Configuration.GetCartsServiceGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client)
    .EnableCallContextPropagation();

builder.Services
    .AddGrpcClient<SearchApi.SearchApiClient>(o => { o.Address = builder.Configuration.GetSearchServiceGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client)
    .EnableCallContextPropagation();

builder.Services
    .AddGrpcClient<MediaApi.MediaApiClient>(o => { o.Address = builder.Configuration.GetMediaServiceGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client)
    .EnableCallContextPropagation(); 

// Health
builder.Services.AddHealthChecks()
    .AddRabbitMQ(builder.Configuration.GetRabbitUri(), tags: new[] {"ready"}, timeout:TimeSpan.FromSeconds(1));

// Ports
builder.WebHost.ConfigureKestrel(opt => {
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "5000"), listen => listen.Protocols = HttpProtocols.Http1AndHttp2);
    // Operate one port in HTTP/2 mode for GRPC
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("GRPC_PORT") ?? "5001"), listen => listen.Protocols = HttpProtocols.Http2);
});

// Run
var app = builder.Build();
app.UseAliveEndpoint();
app.UseReadyEndpoint();
app.MapGrpcService<ShoppingApiService>();
app.Run();
