using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class NginxExtensions
    {
        private const string DefaultNginxScheme = "http";
        private const string DefaultNginxHost = "localhost";
        private const int DefaultNginxPort = 80;
        
        public static string GetNginxScheme(this IConfiguration configuration) => 
            configuration.GetString("INFRA_NGINX_SCHEME", DefaultNginxScheme);
        public static string GetNginxHost(this IConfiguration configuration) => 
            configuration.GetString("INFRA_NGINX_HOST",DefaultNginxHost);
        public static int GetNginxHostPort(this IConfiguration configuration) => 
            configuration.GetInt("INFRA_NGINX_PORT", DefaultNginxPort);
        
        public static Uri GetNginxUri(this IConfiguration configuration)
        {
            var scheme = configuration.GetNginxScheme();
            var host = configuration.GetNginxHost();
            var port = configuration.GetNginxHostPort();
            return new Uri($"{scheme}://{host}:{port}");
        }
    }
}