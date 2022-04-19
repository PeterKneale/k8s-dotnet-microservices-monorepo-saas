using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.HealthChecks;
using Media.Api;
using Media.Application;
using Media.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
{
    Console.WriteLine($"{env.Key}: {env.Value}");
}

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

builder.Services.AddGrpc(c => {
    c.EnableDetailedErrors = true;
});

builder.Services.AddSingleton<IMinioSetup, MinioSetup>();
builder.Services.AddSingleton<IGalleryService, GalleryService>();
builder.Services.AddSingleton<IGalleryUploadService, GalleryUploadService>();
builder.Services.AddSingleton<IMinioClientFactory, MinioClientFactory>();

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri(builder.Configuration.GetMinioUri(), new Uri("/minio/health/live", UriKind.Relative)), tags: new[] {"ready"}, timeout: TimeSpan.FromSeconds(1));

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
app.MapGrpcService<MediaApiService>();

var service = app.Services.GetRequiredService<IMinioSetup>();
service.CreateBuckets().GetAwaiter().GetResult();

app.Run();