using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BuildingBlocks.Infrastructure.HealthChecks
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthCheckPublisher(this IServiceCollection services) => services
            .Configure<HealthCheckPublisherOptions>(options => { options.Predicate = check => check.Tags.Contains(HealthCheckConstants.HealthCheckReadyTag); });
    }
}