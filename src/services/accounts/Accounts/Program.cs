using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.ExceptionHandling;
using BuildingBlocks.Infrastructure.HealthChecks;
using Accounts.Api;
using Accounts.Domain;
using Accounts.Infrastructure.Behaviors;
using Accounts.Infrastructure.Repository;
using BuildingBlocks.Infrastructure.AccountContext;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using FluentValidation;
using Marten;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Weasel.Postgresql;

// Enable W3C Trace Context support for distributed tracing
Activity.DefaultIdFormat = ActivityIdFormat.W3C;

// Allow grpc to operate in a non-TLS environment
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

// Build
var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddLogging(c => {
    c.AddJsonConsole(options => {
        options.IncludeScopes = true;
        options.JsonWriterOptions = new JsonWriterOptions
        {
            Indented = false
        };
    });
});

// Application
var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddMediatR(assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(assembly);

// MultiAccount
builder.Services.AddScopedAccountContext();

// Infrastructure
builder.Services.AddScoped<IWriteRepository, WriteRepository>();
builder.Services.AddScoped<IReadRepository, ReadRepository>();
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetPostgresConnectionString());
    options.DatabaseSchemaName = builder.Configuration.GetPostgresSchema();
    options.AutoCreateSchemaObjects = AutoCreate.All;
    options.Serializer(MartenSerializer.GetSerializer());
    options.Logger(new ConsoleMartenLogger());
    options.Schema.For<Accounts.Domain.Account>().Identity(x => x.AccountId).DocumentAlias("accounts");
    options.Schema.For<Accounts.Domain.User>().Identity(x => x.UserId).DocumentAlias("users");
});

// Bus
builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration.GetRabbitUri());
        cfg.ConfigureEndpoints(context);
    });
});

// GRPC Server
builder.Services.AddGrpc(c => {
    c.Interceptors.Add<GrpcExceptionInterceptor>();
    c.Interceptors.Add<AccountContextAwareServerInterceptor>();
    c.EnableDetailedErrors = true;
});

// Health
builder.Services.AddHealthChecks()
    //.AddNpgSql(builder.Configuration.GetPostgresConnectionString(), tags: new[] {"ready"}, timeout: TimeSpan.FromSeconds(1))
    .AddRabbitMQ(builder.Configuration.GetRabbitUri(), tags: new[] {"ready"}, timeout: TimeSpan.FromSeconds(1));

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

app.MapGrpcService<AccountsApiService>();
app.Run();