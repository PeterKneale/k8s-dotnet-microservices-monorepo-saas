using BuildingBlocks.Infrastructure.HealthChecks;
using Management.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using BuildingBlocks.Infrastructure.AccountContext;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using BuildingBlocks.Infrastructure.Configuration;
using Catalog.Api;
using FluentValidation;
using Grpc.Net.ClientFactory;
using Management.Infrastructure.Behaviors;
using Media;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Stores.Api;

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

// application
var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddMediatR(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);

// infra
builder.Services.AddHealthChecks();
builder.WebHost.ConfigureKestrel(opt => {
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT") ?? "5000"), listen => listen.Protocols = HttpProtocols.Http1AndHttp2);
});
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
});
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddAccountContextAwareClientInterceptor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAccountContextGetter, AccountContextGetter>();

// grpc clients
builder.Services
    .AddGrpcClient<CatalogApi.CatalogApiClient>(o => { o.Address = builder.Configuration.GetCatalogServiceGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client);

builder.Services
    .AddGrpcClient<StoresApi.StoresApiClient>(o => { o.Address = builder.Configuration.GetStoresServiceGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client);

builder.Services
    .AddGrpcClient<MediaApi.MediaApiClient>(o => { o.Address = builder.Configuration.GetMediaServiceGrpcUri(); })
    .AddInterceptor<AccountContextAwareClientInterceptor>(InterceptorScope.Client);

// auth
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