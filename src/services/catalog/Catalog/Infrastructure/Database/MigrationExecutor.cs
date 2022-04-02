using System;
using System.Reflection;
using BuildingBlocks.Infrastructure.Configuration;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Database
{
    public class MigrationExecutor
    {
        private readonly IConfiguration _configuration;

        public MigrationExecutor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Reset()
        {
            Down();
            Up();
        }

        public void Up() => Configure(x => x.MigrateUp());

        public void Down() => Configure(x => x.MigrateDown(0));

        public void Configure(Action<IMigrationRunner> action)
        {
            using var provider = GetProvider(_configuration);
            using var scope = provider.CreateScope();
            var runner = provider.GetRequiredService<IMigrationRunner>();
            action(runner);
        }

        private static ServiceProvider GetProvider(IConfiguration configuration)
        {
            var connection = configuration.GetPostgresConnectionString();
            var schema = configuration.GetPostgresSchema();
            var assemblies = new[] {Assembly.GetExecutingAssembly()};
            var conventionSet = new DefaultConventionSet(schema, null);
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => { 
                    builder
                        .AddPostgres()
                        .WithGlobalConnectionString(connection)
                        .ScanIn(assemblies).For.All();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddSingleton<IConventionSet>(conventionSet)
                .BuildServiceProvider(false);
        }
    }
}