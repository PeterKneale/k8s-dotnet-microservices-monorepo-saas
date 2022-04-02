using Microsoft.Extensions.Configuration;

namespace Management.Application
{
    public static class ConfigurationExtensions
    {
        public static string[] GetThemes(this IConfiguration configuration)
        {
            return new[] {"cerulean", "cosmo", "cyborg", "default", "quartz"};
        }
        
        public static string GetParentDomain(this IConfiguration configuration)
        {
            return "saas.io";
        }
    }
}