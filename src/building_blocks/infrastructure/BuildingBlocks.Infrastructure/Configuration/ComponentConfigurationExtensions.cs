using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class ComponentConfigurationExtensions
    {
        // Backends
        private const string FRONTEND = "FRONTEND";
        
        // Backends
        private const string BACKEND = "BACKEND";
        public static Uri GetRegistrationBackendGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(BACKEND,"REGISTRATION");
        public static Uri GetShoppingBackendGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(BACKEND, "SHOPPING");

        // Services
        private const string SERVICE = "SERVICE";
        public static Uri GetCartsServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(SERVICE, "CARTS");
        public static Uri GetCatalogServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(SERVICE,"CATALOG");
        public static Uri GetMediaServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(SERVICE,"MEDIA");
        public static Uri GetSearchServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(SERVICE,"SEARCH");
        public static Uri GetStoresServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(SERVICE,"STORES");
        public static Uri GetAccountsServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(SERVICE,"ACCOUNTS");

        // wrappers
        public static Uri GetFrontendHttpUri(this IConfiguration configuration, string name) => new Uri($"http://{configuration.GetHost(FRONTEND, name)}:{configuration.GetHttpPort(FRONTEND, name)}");
        public static Uri GetBackendHttpUri(this IConfiguration configuration, string name) => new Uri($"http://{configuration.GetHost(BACKEND, name)}:{configuration.GetHttpPort(BACKEND, name)}");
        public static Uri GetServiceHttpUri(this IConfiguration configuration, string name) => new Uri($"http://{configuration.GetHost(SERVICE, name)}:{configuration.GetHttpPort(SERVICE, name)}");
        public static Uri GetHttpUri(this IConfiguration configuration, string type, string name) => new Uri($"http://{configuration.GetHost(type, name)}:{configuration.GetHttpPort(type, name)}");
        public static Uri GetGrpcUri(this IConfiguration configuration, string type, string name) => new Uri($"http://{configuration.GetHost(type, name)}:{configuration.GetGrpcPort(type, name)}");

        private const string DefaultHost = "localhost";
        private const int DefaultHttpPort = 5000;
        private const int DefaultGrpcPort = 5001;
        public static string GetHost(this IConfiguration configuration, string type, string name) => configuration.GetString($"{type}_{name}_HOST", DefaultHost);
        public static int GetHttpPort(this IConfiguration configuration, string type, string name) => configuration.GetInt($"{type}_{name}_PORT_HTTP", DefaultHttpPort);
        public static int GetGrpcPort(this IConfiguration configuration, string type, string name) => configuration.GetInt($"{type}_{name}_PORT_GRPC", DefaultGrpcPort);

        // utility
        public static Uri GetUri(this IConfiguration configuration, string key, string defaultValue) => new(configuration.GetString(key, defaultValue));
        public static int GetInt(this IConfiguration configuration, string key, int defaultValue) => int.Parse(configuration.GetString(key, defaultValue.ToString()));
        public static bool GetBool(this IConfiguration configuration, string key, bool defaultValue) => bool.Parse(configuration.GetString(key, defaultValue.ToString()));
        public static string GetString(this IConfiguration configuration, string key, string defaultValue) => configuration[key] ?? defaultValue;
    }
}