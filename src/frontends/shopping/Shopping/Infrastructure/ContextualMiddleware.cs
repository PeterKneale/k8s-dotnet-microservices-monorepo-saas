using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Shopping.Infrastructure
{
    internal class ContextualMiddleware
    {
        private readonly RequestDelegate _next;

        public ContextualMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var service = httpContext.RequestServices.GetRequiredService<IContextualMiddlewareService>();
            var logs = httpContext.RequestServices.GetRequiredService<ILogger<ContextualMiddleware>>();
            try
            {
                await service.SetContext(httpContext);
                await _next(httpContext);
            }
            catch (Exception e)
            {
                logs.LogError(e, "Error setting context");
                var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();
                var location = configuration["ContextualMiddlewareErrorRedirect"];
                logs.LogInformation($"Context cannot be set so redirecting to {location}");
                httpContext.Response.Redirect(location);
                await httpContext.Response.CompleteAsync();
            }
        }
    }
}