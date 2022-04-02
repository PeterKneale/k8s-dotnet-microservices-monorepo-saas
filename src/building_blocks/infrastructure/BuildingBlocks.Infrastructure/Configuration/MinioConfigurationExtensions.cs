using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class MinioConfigurationExtensions
    {
        private const string DefaultMinioScheme = "http";
        private const string DefaultMinioHost = "infra_minio";
        private const int DefaultMinioPort = 9000;
        private const string DefaultMinioUserName = "admin";
        private const string DefaultMinioPassword = "password";
        private const string DefaultMinioTraceEnabled = "false";

        public static string GetMinioScheme(this IConfiguration configuration) => configuration.GetString("infra_minio_scheme", DefaultMinioScheme);
        public static string GetMinioHost(this IConfiguration configuration) => configuration.GetString("INFRA_MINIO_SERVICE_HOST", DefaultMinioHost);
        public static int GetMinioPort(this IConfiguration configuration) => configuration.GetInt("INFRA_MINIO_SERVICE_PORT", DefaultMinioPort);
        public static string GetMinioAccessKey(this IConfiguration configuration) => configuration.GetString("infra_minio_access_key", DefaultMinioUserName);
        public static string GetMinioSecretKey(this IConfiguration configuration) => configuration.GetString("infra_minio_secret_key", DefaultMinioPassword);
        public static bool GetMinioTraceEnabled(this IConfiguration configuration) => configuration.GetBool("infra_minio_trace_enabled", false);

        public static Uri GetMinioUri(this IConfiguration configuration)
        {
            var scheme = configuration.GetMinioScheme();
            var host = configuration.GetMinioHost();
            var port = configuration.GetMinioPort();
            return new Uri($"{scheme}://{host}:{port}");
        }
    }
}