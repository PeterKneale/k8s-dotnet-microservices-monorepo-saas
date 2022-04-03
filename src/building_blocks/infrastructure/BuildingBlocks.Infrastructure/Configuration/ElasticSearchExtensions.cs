using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class ElasticSearchConfigurationExtensions
    {
        private const string DefaultElasticSearchScheme = "http";
        private const string DefaultElasticSearchHost = "localhost";
        private const int DefaultElasticSearchPort = 9200;

        private static string GetElasticSearchScheme(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_ELASTICSEARCH_SCHEME", DefaultElasticSearchScheme);

        private static string GetElasticSearchHost(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_ELASTICSEARCH_HOST",DefaultElasticSearchHost);

        private static int GetElasticSearchHostPort(this IConfiguration configuration) => 
            configuration.GetInt("SAAS_INFRA_ELASTICSEARCH_PORT", DefaultElasticSearchPort);
        
        public static Uri GetElasticSearchUri(this IConfiguration configuration)
        {
            var scheme = configuration.GetElasticSearchScheme();
            var host = configuration.GetElasticSearchHost();
            var port = configuration.GetElasticSearchHostPort();
            return new Uri($"{scheme}://{host}:{port}");
        }
    }
}