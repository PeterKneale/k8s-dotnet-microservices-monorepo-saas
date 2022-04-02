using System;
using Microsoft.Extensions.Configuration;

namespace SystemTests.Helpers
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Loads configuration from test settings.json
        /// Loads additional configuration from appsettings.development.json when running locally in development mode
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static string? EnvironmentName =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}