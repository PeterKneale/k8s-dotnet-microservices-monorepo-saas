using System;
using System.Diagnostics;
using System.Net;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.HealthChecks;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Registration.Api;
using Registration.Application;
using Registration.Consumers;
using Registration.Infrastructure;
using Stores.Api;
using Accounts.Api;

// Enable W3C Trace Context support for distributed tracing
Activity.DefaultIdFormat = ActivityIdFormat.W3C;

// Allow grpc to operate in a non-TLS environment
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddLogging(c => {
    c.AddJsonConsole();
});

builder.Services.AddGrpc(c => {
    c.EnableDetailedErrors = true;
});

builder.Services.AddHealthChecks()
    .AddRabbitMQ(builder.Configuration.GetRabbitUri(), tags: new[] {"ready"}, timeout: TimeSpan.FromSeconds(1));

builder.WebHost.ConfigureKestrel(opt => {
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "5000"), listen => listen.Protocols = HttpProtocols.Http1AndHttp2);
    // Operate one port in HTTP/2 mode for GRPC
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("GRPC_PORT") ?? "5001"), listen => listen.Protocols = HttpProtocols.Http2);
});

builder.Services.AddMassTransit(x => {
    x.AddConsumer<RegistrationRequestedConsumer>();
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration.GetRabbitUri());
        cfg.ConfigureEndpoints(context);
        cfg.ReceiveEndpoint(Constants.QueueName, e => {
            e.UseRetry(r => {
                r.Interval(3, TimeSpan.FromMilliseconds(100));
            });
            e.ConfigureConsumer<RegistrationRequestedConsumer>(context);
        });
    });
});
builder.Services.AddMassTransitHostedService();

builder.Services.AddGrpcClient<StoresApi.StoresApiClient>(o => { o.Address = builder.Configuration.GetStoresServiceGrpcUri(); });
builder.Services.AddGrpcClient<AccountsApi.AccountsApiClient>(o => { o.Address = builder.Configuration.GetAccountsServiceGrpcUri(); });
builder.Services.AddScoped<IRegistrationQueue, RegistrationQueue>();
builder.Services.AddScoped<IStoresServiceGateway, StoresServiceGateway>();
builder.Services.AddScoped<IAccountsServiceGateway, AccountsServiceGateway>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

var app = builder.Build();
app.UseAliveEndpoint();
app.UseReadyEndpoint();
app.MapGrpcService<RegistrationApiService>();
app.Run();