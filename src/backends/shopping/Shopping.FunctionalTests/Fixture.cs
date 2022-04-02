using Grpc.Net.Client;
using Marten;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Shopping.Api;
using Shopping.Application.Data;
using Shopping.Application.Services;
using Shopping.Infrastructure.DataSources;
using Xunit.Abstractions;

namespace Shopping.FunctionalTests
{
    public class Fixture : WebApplicationFactory<ShoppingApiService>, ITestOutputHelperAccessor
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
            CartsService = new Mock<ICartsServiceGateway>(MockBehavior.Strict);
            CatalogService = new Mock<ICatalogServiceGateway>(MockBehavior.Strict);
            SearchService = new Mock<ISearchServiceGateway>(MockBehavior.Strict);
            
            // Reset database
            using var store = Server.Services.GetRequiredService<IDocumentStore>();
            using var session = store.LightweightSession();
            session.HardDeleteWhere<AccountData>(x => true);
            session.HardDeleteWhere<StoreData>(x => true);
            session.HardDeleteWhere<ProductData>(x => true);
            session.SaveChanges();
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(p => p.AddXUnit(this));
            builder.ConfigureTestServices(x => {
                x.AddScoped(_=>CartsService.Object);
                x.AddScoped(_=>CatalogService.Object);
                x.AddScoped(_=>SearchService.Object);
            });
        }
        
        public ITestOutputHelper? OutputHelper { get; set; }
        public Mock<ICartsServiceGateway> CartsService { get; }
        public Mock<ICatalogServiceGateway> CatalogService { get; }
        public Mock<ISearchServiceGateway> SearchService { get; }
        public ShoppingApi.ShoppingApiClient GetClient() => new(_channel);

        protected override void Dispose(bool disposing)
        {
            _channel?.Dispose();
            base.Dispose(disposing);
        }
    }
}