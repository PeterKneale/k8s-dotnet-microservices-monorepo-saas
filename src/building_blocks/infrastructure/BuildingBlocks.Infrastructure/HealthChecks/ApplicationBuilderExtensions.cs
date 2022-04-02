using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace BuildingBlocks.Infrastructure.HealthChecks
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAliveEndpoint(this IApplicationBuilder app) =>
            app.UseHealthChecks(HealthCheckConstants.HealthCheckAliveRoute, new HealthCheckOptions
            {
                // Exclude all checks and return a 200-Ok.
                Predicate = _ => false
            });

        public static IApplicationBuilder UseReadyEndpoint(this IApplicationBuilder app) =>
            app.UseHealthChecks(HealthCheckConstants.HealthCheckReadyRoute, new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(HealthCheckConstants.HealthCheckReadyTag)
            });
    }
}