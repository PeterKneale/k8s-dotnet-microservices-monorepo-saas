using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class RabbitConfigurationExtensions
    {
        private const string DefaultRabbitHost = "infra_rabbit";
        private const string DefaultRabbitUserName = "admin";
        private const string DefaultRabbitPassword = "password";
        private const int DefaultRabbitPort = 5672;
        private const string DefaultRabbitVirtualHost = "";
        
        public static string GetRabbitHost(this IConfiguration configuration) => configuration.GetString("INFRA_RABBITMQ_SERVICE_HOST", DefaultRabbitHost);
        public static string GetRabbitUserName(this IConfiguration configuration) => configuration.GetString("infra_rabbit_username", DefaultRabbitUserName);
        public static string GetRabbitPassword(this IConfiguration configuration) => configuration.GetString("infra_rabbit_password", DefaultRabbitPassword);
        public static int GetRabbitPort(this IConfiguration configuration) => configuration.GetInt("INFRA_RABBITMQ_SERVICE_PORT_AMQP", DefaultRabbitPort);
        public static string GetRabbitVirtualHost(this IConfiguration configuration) => configuration.GetString("infra_rabbit_vhost", DefaultRabbitVirtualHost);
        
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