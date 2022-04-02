using Grpc.Net.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Search.Api;
using Search.Application.Services;
using Xunit.Abstractions;

namespace Search.FunctionalTests
{
    public class Fixture : WebApplicationFactory<SearchApiService>, ITestOutputHelperAccessor
    {
        private readonly GrpcChannel _channel;

        public Fixture()
        {
            var client = CreateDefaultClient();
            _channel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
            
            using var scope = Server.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IIndexManagementService>();
            service.ReCreateIndex();
        }
        
        public ITestOutputHelper? OutputHelper { get; set; }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(x => x.AddXUnit(this));
        }
        
        public SearchApi.SearchApiClient GetClient() => new(_channel);

        protected override void Dispose(bool disposing)
        {
            _channel?.Dispose();
            base.Dispose(disposing);
        }
    }
}