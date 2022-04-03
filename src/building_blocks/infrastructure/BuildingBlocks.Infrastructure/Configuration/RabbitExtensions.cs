using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class RabbitExtensions
    {
        private const string DefaultRabbitHost = "localhost";
        private const string DefaultRabbitUserName = "admin";
        private const string DefaultRabbitPassword = "password";
        private const int DefaultRabbitPort = 5672;
        private const string DefaultRabbitVirtualHost = "";
        
        public static string GetRabbitHost(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_RABBITMQ_HOST", DefaultRabbitHost);
        public static string GetRabbitUserName(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_RABBITMQ_USERNAME", DefaultRabbitUserName);
        public static string GetRabbitPassword(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_RABBITMQ_PASSWORD", DefaultRabbitPassword);
        private static int GetRabbitPort(this IConfiguration configuration) => 
            configuration.GetInt("SAAS_INFRA_RABBITMQ_PORT", DefaultRabbitPort);
        private static string GetRabbitVirtualHost(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_RABBITMQ_VHOST", DefaultRabbitVirtualHost);
        
        public static Uri GetRabbitUri(this IConfiguration configuration)
        {
            var host = configuration.GetRabbitHost();
            var username = configuration.GetRabbitUserName();
            var password = configuration.GetRabbitPassword();
            var port = configuration.GetRabbitPort();
            var vhost = configuration.GetRabbitVirtualHost();
            return new Uri($"amqp://{username}:{password}@{host}:{port}/{vhost}");
        }
    }
}