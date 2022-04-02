using Grpc.Net.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stores.Api;
using Stores.Infrastructure.Database;
using Xunit.Abstractions;

namespace Stores.FunctionalTests
{
    public class Fixture : WebApplicationFactory<StoresApiService>, ITestOutputHelperAccessor
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
            builder.ConfigureLogging(x => x.AddXUnit(this));
        }
        
        public StoresApi.StoresApiClient GetClient() => new(_channel);

        protected override void Dispose(bool disposing)
        {
            _channel?.Dispose();
            base.Dispose(disposing);
        }
    }
}