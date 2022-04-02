using Grpc.Net.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Registration.Api;
using Registration.Application;
using Xunit.Abstractions;

namespace Registration.FunctionalTests
{
    public class Fixture : WebApplicationFactory<RegistrationApiService>, ITestOutputHelperAccessor
    {
        private readonly GrpcChannel _channel;

        public Fixture()
        {
            var client = CreateDefaultClient();
            _channel = GrpcChannel.ForAddress(client.BaseAddress!, new GrpcChannelOptions
            {
                HttpClient = client
            });

            // Created Mocked version of services
            StoresService = new Mock<IStoresServiceGateway>(MockBehavior.Strict);
            AccountsService = new Mock<IAccountsServiceGateway>(MockBehavior.Strict);
            
            // TODO: rabbit needs to be purged between tests
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(x => {
                x.AddScoped(_ => AccountsService.Object);
                x.AddScoped(_ => StoresService.Object);
            });
            builder.ConfigureLogging(p => p.AddXUnit(this));
        }

        public ITestOutputHelper? OutputHelper { get; set; }
        public Mock<IAccountsServiceGateway> AccountsService { get; }
        public Mock<IStoresServiceGateway> StoresService { get; }
        public RegistrationApi.RegistrationApiClient GetClient() => new(_channel);

        protected override void Dispose(bool disposing)
        {
            _channel?.Dispose();
            base.Dispose(disposing);
        }
    }
}