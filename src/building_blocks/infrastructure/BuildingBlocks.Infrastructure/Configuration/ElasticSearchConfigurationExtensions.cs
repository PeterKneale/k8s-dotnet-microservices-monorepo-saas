using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class ElasticSearchConfigurationExtensions
    {
        private const string DefaultElasticSearchScheme = "http";
        private const string DefaultElasticSearchHost = "localhost";
        private const int DefaultElasticSearchPort = 9200;
        
        public static string GetElasticSearchScheme(this IConfiguration configuration) => 
            configuration.GetString("INFRA_ELASTICSEARCH_SCHEME", DefaultElasticSearchScheme);
        
        public static string GetElasticSearchHost(this IConfiguration configuration) => 
            configuration.GetString("INFRA_ELASTICSEARCH_HOST",DefaultElasticSearchHost);
        
        public static int GetElasticSearchHostPort(this IConfiguration configuration) => 
            configuration.GetInt("INFRA_ELASTICSEARCH_PORT", DefaultElasticSearchPort);
        
        public static Uri GetElasticSearchUri(this IConfiguration configuration)
        {
            var scheme = configuration.GetElasticSearchScheme();
            var host = configuration.GetElasticSearchHost();
            var port = configuration.GetElasticSearchHostPort();
            return new Uri($"{scheme}://{host}:{port}");
        }
    }
}