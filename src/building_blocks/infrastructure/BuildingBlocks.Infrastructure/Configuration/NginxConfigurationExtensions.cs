using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class NginxConfigurationExtensions
    {
        private const string DefaultNginxScheme = "http";
        private const string DefaultNginxHost = "ingress_public";
        private const int DefaultNginxPort = 80;
        
        public static string GetNginxScheme(this IConfiguration configuration) => configuration.GetString("infra_nginx_scheme", DefaultNginxScheme);
        public static string GetNginxHost(this IConfiguration configuration) => configuration.GetString("infra_nginx_host",DefaultNginxHost);
        public static int GetNginxHostPort(this IConfiguration configuration) => configuration.GetInt("infra_nginx_port", DefaultNginxPort);
        
        public static Uri GetNginxUri(this IConfiguration configuration)
        {
            var scheme = configuration.GetNginxScheme();
            var host = configuration.GetNginxHost();
            var port = configuration.GetNginxHostPort();
            return new Uri($"{scheme}://{host}:{port}");
        }
    }
}