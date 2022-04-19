using System;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class ServiceExtensions
    {
        private const string Frontend = "FRONTEND";
        private const string Backend = "BACKEND";
        private const string Service = "SERVICE";

        // Http endpoints
        public static Uri GetFrontendHttpUri(this IConfiguration configuration, string name) => configuration.GetHttpUri(Frontend, name);
        public static Uri GetBackendHttpUri(this IConfiguration configuration, string name) => configuration.GetHttpUri(Backend, name);
        public static Uri GetServiceHttpUri(this IConfiguration configuration, string name) => configuration.GetHttpUri(Service, name);

        // Grpc Endpoints
        public static Uri GetRegistrationBackendGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Backend, "REGISTRATION");
        public static Uri GetShoppingBackendGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Backend, "SHOPPING");
        public static Uri GetCartsServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Service, "CARTS");
        public static Uri GetCatalogServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Service, "CATALOG");
        public static Uri GetMediaServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Service, "MEDIA");
        public static Uri GetSearchServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Service, "SEARCH");
        public static Uri GetStoresServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Service, "STORES");
        public static Uri GetAccountsServiceGrpcUri(this IConfiguration configuration) => configuration.GetGrpcUri(Service, "ACCOUNTS");

        // common
        private const string DefaultHost = "localhost";
        private const int DefaultHttpPort = 5000;
        private const int DefaultGrpcPort = 5001;

        private static Uri GetGrpcUri(this IConfiguration configuration, string type, string name) =>
            new($"http://{configuration.GetHost(type, name)}:{configuration.GetGrpcPort(type, name)}");

        private static Uri GetHttpUri(this IConfiguration configuration, string type, string name) =>
            new($"http://{configuration.GetHost(type, name)}:{configuration.GetHttpPort(type, name)}");

        private static string GetHost(this IConfiguration configuration, string type, string name) =>
            configuration.GetString($"SAAS_{type}_{name}_HOST", DefaultHost);

        private static int GetHttpPort(this IConfiguration configuration, string type, string name) =>
            configuration.GetInt($"SAAS_{type}_{name}_PORT_HTTP", DefaultHttpPort);

        private static int GetGrpcPort(this IConfiguration configuration, string type, string name) =>
            configuration.GetInt($"SAAS_{type}_{name}_PORT_GRPC", DefaultGrpcPort);
    }
}