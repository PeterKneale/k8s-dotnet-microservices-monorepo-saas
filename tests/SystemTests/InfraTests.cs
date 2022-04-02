using System.Net.Http;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.Configuration;
using FluentAssertions;
using Npgsql;
using RabbitMQ.Client;
using SystemTests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SystemTests
{
    public class InfraTests : TestBase, IClassFixture<Fixture>
    {
        private readonly ITestOutputHelper _output;

        public InfraTests(Fixture container, ITestOutputHelper output) : base(container)
        {
            _output = output;
        }

        [Fact]
        public async Task Postgres_can_be_connected_to()
        {
            // arrange
            var connectionString = ConfigurationHelper.Configuration.GetPostgresConnectionString();

            // act
            await using var connection = new NpgsqlConnection(connectionString);
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT version();";
            await connection.OpenAsync();
            var result = (await command.ExecuteScalarAsync()) as string;
            await connection.CloseAsync();

            // debug
            _output.WriteLine(result);

            // assert
            result.Should().Contain("PostgreSQL");
        }

        [Fact]
        public async Task Elastic_search_can_be_connected_to()
        {
            // arrange
            var address = ConfigurationHelper.Configuration.GetElasticSearchUri();

            // act
            var client = new HttpClient();
            client.BaseAddress = address;
            var result = await client.GetStringAsync("/");

            // debug
            _output.WriteLine(result);

            // assert
            result.Should().Contain("You Know, for Search");
        }

        [Fact]
        public void Rabbit_can_be_connected_to()
        {
            // arrange
            var address = ConfigurationHelper.Configuration.GetRabbitUri();

            // act
            var factory = new ConnectionFactory()
            {
                Uri = address
            };
            var connection = factory.CreateConnection();
            var properties = connection.ServerProperties;

            // debug
            foreach (var (key, value) in properties)
            {
                _output.WriteLine($"{key} = {value}");
            }

            // assert
            properties.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Ingress_can_be_connected_to()
        {
            // arrange
            var address = ConfigurationHelper.Configuration.GetRabbitUri();

            // act
            var factory = new ConnectionFactory()
            {
                Uri = address
            };
            var connection = factory.CreateConnection();
            var properties = connection.ServerProperties;

            // debug
            foreach (var (key, value) in properties)
            {
                _output.WriteLine($"{key} = {value}");
            }

            // assert
            properties.Should().NotBeEmpty();
        }
    }
}