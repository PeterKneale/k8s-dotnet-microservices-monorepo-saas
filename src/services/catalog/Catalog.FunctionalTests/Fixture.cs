using Catalog.Api;
using Catalog.Infrastructure.Database;
using Grpc.Net.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Catalog.FunctionalTests
{
    public class Fixture : WebApplicationFactory<CatalogApiService>, ITestOutputHelperAccessor
    {
        private readonly GrpcChannel _channel;

        public Fixture()
        {
            var client = CreateDefaultClient();
            _channel = GrpcChannel.ForAddress(client.BaseAddress!, new GrpcChannelOptions
            {
                HttpClient = client
            });
            var migrator =  Server.Services.GetRequiredService<MigrationExecutor>();
            migrator.Reset();
        }
        
        public ITestOutputHelper? OutputHelper { get; set; }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging((p) => p.AddXUnit(this));
        }
        
        public CatalogApi.CatalogApiClient GetClient() => new(_channel);

        protected override void Dispose(bool disposing)
        {
            _channel?.Dispose();
            base.Dispose(disposing);
        }
    }
}