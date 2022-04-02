using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Infrastructure.Configuration
{
    public static class PostgresConfigurationExtensions
    {
        // Postgres
        private const string DefaultDatabaseHost = "infra_postgres";
        private const string DefaultDatabaseName = "saas";
        private const string DefaultDatabaseSchema = "public";
        private const string DefaultDatabaseUserName = "admin";
        private const string DefaultDatabasePassword = "password";
        
        public static string GetPostgresHost(this IConfiguration configuration) => configuration.GetString("INFRA_POSTGRESQL_SERVICE_HOST", DefaultDatabaseHost);
        public static string GetPostgresDatabase(this IConfiguration configuration) => configuration.GetString("infra_postgres_database",DefaultDatabaseName);
        public static string GetPostgresSchema(this IConfiguration configuration) => configuration.GetString("infra_postgres_schema", DefaultDatabaseSchema);
        public static string GetPostgresUserName(this IConfiguration configuration) => configuration.GetString("infra_postgres_username", DefaultDatabaseUserName);
        public static string GetPostgresPassword(this IConfiguration configuration) => configuration.GetString("infra_postgres_password", DefaultDatabasePassword);
        
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