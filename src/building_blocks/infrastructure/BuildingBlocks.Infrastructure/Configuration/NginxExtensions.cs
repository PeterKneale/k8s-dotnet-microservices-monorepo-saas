﻿using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class NginxExtensions
    {
        private const string DefaultNginxScheme = "http";
        private const string DefaultNginxHost = "localhost";
        private const int DefaultNginxPort = 80;

        private static string GetNginxScheme(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_NGINX_SCHEME", DefaultNginxScheme);
        
        private static string GetNginxHost(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_NGINX_HOST",DefaultNginxHost);
        
        private static int GetNginxHostPort(this IConfiguration configuration) => 
            configuration.GetInt("SAAS_INFRA_NGINX_PORT", DefaultNginxPort);
        
        public static Uri GetNginxUri(this IConfiguration configuration)
        {
            var scheme = configuration.GetNginxScheme();
            var host = configuration.GetNginxHost();
            var port = configuration.GetNginxHostPort();
            return new Uri($"{scheme}://{host}:{port}");
        }
    }
}