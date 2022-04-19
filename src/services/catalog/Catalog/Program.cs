using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using BuildingBlocks.Infrastructure.AccountContext;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.ExceptionHandling;
using BuildingBlocks.Infrastructure.HealthChecks;
using Catalog.Api;
using Catalog.Domain;
using Catalog.Infrastructure.Behaviors;
using Catalog.Infrastructure.Database;
using Catalog.Infrastructure.Repository;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

// Infrastructure
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>(x => 
    new DbConnectionFactory(builder.Configuration.GetPostgresConnectionString())
);

// MultiAccount
builder.Services.AddScopedAccountContext();

// Database migrations
builder.Services.AddSingleton(new MigrationExecutor( builder.Configuration));

// GRPC Server
builder.Services.AddGrpc(c => {
    c.Interceptors.Add<GrpcExceptionInterceptor>();
    c.Interceptors.Add<AccountContextAwareServerInterceptor>();
    c.EnableDetailedErrors = true;
});

// Health
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetPostgresConnectionString(), tags: new[] {"ready"}, timeout:TimeSpan.FromSeconds(1))
    .AddRabbitMQ(builder.Configuration.GetRabbitUri(), tags: new[] {"ready"}, timeout:TimeSpan.FromSeconds(1));

// Bus
builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration.GetRabbitHost(), "/", h => {
            h.Username(builder.Configuration.GetRabbitUserName());
            h.Password(builder.Configuration.GetRabbitPassword());
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddMassTransitHostedService();

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
var executor = app.Services.GetRequiredService<MigrationExecutor>();
executor.Up();

app.MapGrpcService<CatalogApiService>();
app.Run();
