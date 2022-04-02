using Carts.Api;
using Carts.Infrastructure.Database;
using Grpc.Net.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Carts.FunctionalTests
{
    public class Fixture : WebApplicationFactory<CartsApiService>, ITestOutputHelperAccessor
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
        
        public CartsApi.CartsApiClient GetClient() => new(_channel);

        protected override void Dispose(bool disposing)
        {
            _channel?.Dispose();
            base.Dispose(disposing);
        }
    }
}