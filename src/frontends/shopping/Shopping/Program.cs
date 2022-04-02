using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.HealthChecks;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shopping.Api;
using System.Diagnostics;
using System.Net;
using System;
using BuildingBlocks.Infrastructure.AccountContext;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Shopping.Infrastructure;

// Enable W3C Trace Context support for distributed tracing
Activity.DefaultIdFormat = ActivityIdFormat.W3C;

// Allow grpc to operate in a non-TLS environment
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

// Build
var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddLogging(c => {
    c.AddJsonConsole();
});

// Application

// Infrastructure
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IContextualMiddlewareService, ContextualMiddlewareService>();
builder.Services.AddScoped<IDomainNameGetter, DomainNameGetter>();
builder.Services.AddScoped<IStoreContextLoader, StoreContextLoader>();
builder.Services.AddScoped<IStoreContextSetter, StoreContextSetter>();
builder.Services.AddScoped<IAccountContextGetter, AccountContextGetter>();
builder.Services.AddScoped<IAccountContextSetter, AccountContextSetter>();

// GRPC Client
builder.Services.AddAccountContextAwareClientInterceptor();
builder.Services
    .AddGrpcClient<ShoppingApi.ShoppingApiClient>(o => { o.Address = builder.Configuration.GetShoppingBackendGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client);

// Health
builder.Services.AddHealthChecks();

// Ports
builder.WebHost.ConfigureKestrel(opt => {
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "5000"), listen => listen.Protocols = HttpProtocols.Http1AndHttp2);
    // Operate one port in HTTP/2 mode for GRPC
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("GRPC_PORT") ?? "5001"), listen => listen.Protocols = HttpProtocols.Http2);
});
// Web
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
});
// Run
var app = builder.Build();
app.UseForwardedHeaders();
app.UseAliveEndpoint();
app.UseReadyEndpoint();
app.UseMiddleware<ContextualMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.Run();