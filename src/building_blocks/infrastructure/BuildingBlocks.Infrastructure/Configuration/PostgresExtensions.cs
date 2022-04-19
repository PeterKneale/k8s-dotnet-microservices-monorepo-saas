using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class PostgresExtensions
    {
        private const string DefaultDatabaseHost = "localhost";
        private const string DefaultDatabaseName = "saas";
        private const string DefaultDatabaseSchema = "public";
        private const string DefaultDatabaseUserName = "admin";
        private const string DefaultDatabasePassword = "password";

        private static string GetPostgresHost(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_POSTGRESQL_HOST", DefaultDatabaseHost);

        private static string GetPostgresDatabase(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_POSTGRESQL_DATABASE",DefaultDatabaseName);

        private static string GetPostgresUserName(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_POSTGRESQL_USERNAME", DefaultDatabaseUserName);

        private static string GetPostgresPassword(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_POSTGRESQL_PASSWORD", DefaultDatabasePassword);

        public static string GetPostgresSchema(this IConfiguration configuration) => 
            configuration.GetString("SAAS_INFRA_POSTGRESQL_SCHEMA", DefaultDatabaseSchema);
        
        public static string GetPostgresConnectionString(this IConfiguration configuration)
        {
            var host = configuration.GetPostgresHost();
            var database = configuration.GetPostgresDatabase();
            var schema = configuration.GetPostgresSchema();
            var username = configuration.GetPostgresUserName();
            var password = configuration.GetPostgresPassword();
            return $"Host={host};Database={database};Username={username};Password={password};search path={schema};Include Error Detail=true";
        }
    }
}