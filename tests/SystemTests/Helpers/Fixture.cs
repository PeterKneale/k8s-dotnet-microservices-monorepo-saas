using System;
using System.Diagnostics;
using System.Net.Http;
using BuildingBlocks.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Catalog.Api;
using Registration.Api;
using Shopping.Api;
using Stores.Api;
using Accounts.Api;

namespace SystemTests.Helpers
{
    public class Fixture : IDisposable
    {
        public Fixture()
        {
            // Enable W3C Trace Context support for distributed tracing
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            // Allow grpc to operate in a non-TLS environment
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var configuration = ConfigurationHelper.Configuration;
            var services = new ServiceCollection()
                .AddLogging(x => x.AddConsole());

            services.AddGrpcClient<CatalogApi.CatalogApiClient>(o => {
                o.Address = configuration.GetCatalogServiceGrpcUri();
            });
            services.AddGrpcClient<StoresApi.StoresApiClient>(o => {
                o.Address = configuration.GetStoresServiceGrpcUri();
            });
            services.AddGrpcClient<AccountsApi.AccountsApiClient>(o => {
                o.Address = configuration.GetAccountsServiceGrpcUri();
            });
            services.AddGrpcClient<ShoppingApi.ShoppingApiClient>(o => {
                o.Address = configuration.GetShoppingBackendGrpcUri();
            });
            services.AddGrpcClient<RegistrationApi.RegistrationApiClient>(o => {
                o.Address = configuration.GetRegistrationBackendGrpcUri();
            });

            ServiceProvider = services.BuildServiceProvider();

            HttpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
        }
        
        public IHttpClientFactory HttpClientFactory { get; }

        public ServiceProvider ServiceProvider { get; }

        public void Dispose() => ServiceProvider.Dispose();

    }
}