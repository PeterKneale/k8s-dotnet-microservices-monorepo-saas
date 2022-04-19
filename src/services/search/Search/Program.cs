using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using BuildingBlocks.Infrastructure.AccountContext;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.ExceptionHandling;
using BuildingBlocks.Infrastructure.HealthChecks;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using Search.Api;
using Search.Application.Services;
using Search.Infrastructure.Behaviors;
using Search.Infrastructure.Consumers;
using Search.Infrastructure.Index;

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
    c.AddSimpleConsole(opt => {
        opt.SingleLine = true;
        opt.IncludeScopes = true;
    });
});

// Application
var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddMediatR(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);

// Infrastructure
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IIndexService, IndexService>();
builder.Services.AddScoped<IIndexManagementService, IndexManagementService>();

// MultiAccount
builder.Services.AddScopedAccountContext();

// Bus
builder.Services.AddMassTransit(x => {
    x.AddConsumer<ProductAddedConsumer>();
    x.AddConsumer<ProductUpdatedConsumer>();
    x.AddConsumer<ProductDeletedConsumer>();
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration.GetRabbitUri());
        cfg.ConfigureEndpoints(context);
        cfg.ReceiveEndpoint("service_search", e => {
            e.ConfigureConsumer<ProductAddedConsumer>(context);
            e.ConfigureConsumer<ProductUpdatedConsumer>(context);
            e.ConfigureConsumer<ProductDeletedConsumer>(context);
        });
    });
});
builder.Services.Configure<MassTransitHostOptions>(options =>
{
    options.WaitUntilStarted = true;
    options.StartTimeout = TimeSpan.FromSeconds(30);
    options.StopTimeout = TimeSpan.FromSeconds(30);
});

// Elastic search
var settings = new ConnectionSettings(builder.Configuration.GetElasticSearchUri())
    .DefaultMappingFor<ProductDocument>(i => i
        .IndexName(IndexService.IndexName)
        .IdProperty(p => p.ProductId)
    )
    .EnableDebugMode()
    .PrettyJson();
builder.Services.AddSingleton<IElasticClient>(new ElasticClient(settings));

// GRPC Server
builder.Services.AddGrpc(c => {
    c.Interceptors.Add<GrpcExceptionInterceptor>();
    c.Interceptors.Add<AccountContextAwareServerInterceptor>();
    c.EnableDetailedErrors = true;
});

// Health
builder.Services.AddHealthChecks()
    .AddElasticsearch(builder.Configuration.GetElasticSearchUri().ToString(), name: "elasticsearch", tags: new[] {"ready"}, timeout: TimeSpan.FromSeconds(1))
    .AddRabbitMQ(builder.Configuration.GetRabbitUri(), name: "rabbitmq", tags: new[] {"ready"}, timeout: TimeSpan.FromSeconds(1));

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

// Migrate
using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IIndexManagementService>();
service.EnsureIndexExists();

app.MapGrpcService<SearchApiService>();
app.Run();