using System;
using System.Diagnostics;
using System.Net;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.HealthChecks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Stores.Api;
using Accounts.Api;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var x = 1;

// Enable W3C Trace Context support for distributed tracing
Activity.DefaultIdFormat = ActivityIdFormat.W3C;

// Allow grpc to operate in a non-TLS environment
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

// Build
var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddLogging(c => {
    c.AddSimpleConsole(opt=>{
        opt.SingleLine = true;
        opt.IncludeScopes = true;
    });
});

// Telemetry
builder.Services.AddOpenTelemetryTracing(builder => {
    builder
        .AddSource(Assembly.GetEntryAssembly().GetName().Name)
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddTelemetrySdk()
            .AddEnvironmentVariableDetector())
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddGrpcClientInstrumentation()
        .AddConsoleExporter();
});

builder.Services.AddHealthChecks();
builder.WebHost.ConfigureKestrel(opt => {
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "5000"), listen => listen.Protocols = HttpProtocols.Http1AndHttp2);
});

builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
});

builder.Services.AddGrpcClient<AccountsApi.AccountsApiClient>(o => { o.Address = builder.Configuration.GetAccountsServiceGrpcUri(); });
builder.Services.AddGrpcClient<StoresApi.StoresApiClient>(o => { o.Address = builder.Configuration.GetStoresServiceGrpcUri(); });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Account/Forbidden/";
    });

// Run
var app = builder.Build();
app.UseForwardedHeaders();
app.UseAliveEndpoint();
app.UseReadyEndpoint();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute().RequireAuthorization();
app.Run();
