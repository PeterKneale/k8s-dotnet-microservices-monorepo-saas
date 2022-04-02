using Microsoft.Extensions.Configuration;

namespace Stores.Application
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultDomainSuffix(this IConfiguration configuration)
        {
            return configuration["DEFAULT_DOMAIN_SUFFIX"];
        }
    }
}