using Microsoft.Extensions.Configuration;

namespace Catalog.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static string GetDbConnectionSchema(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("schema");
        }
        
        public static string GetDbConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("connection");
        }
    }
}