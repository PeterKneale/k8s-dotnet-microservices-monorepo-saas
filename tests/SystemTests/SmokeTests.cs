using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api;
using FluentAssertions;
using Google.Type;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Registration.Api;
using Shopping.Api;
using Stores.Api;
using SystemTests.Fakers;
using SystemTests.Helpers;
using Accounts.Api;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Xunit;
using Xunit.Abstractions;

namespace SystemTests
{
    public class SmokeTests : TestBase, IClassFixture<Fixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly AccountsPlatformApi.AccountsPlatformApiClient _accounts;
        private readonly StoresApi.StoresApiClient _stores;
        private readonly CatalogApi.CatalogApiClient _catalog;

        public SmokeTests(Fixture container, ITestOutputHelper output) : base(container)
        {
            _output = output;
            _accounts = ServiceProvider.GetRequiredService<AccountsPlatformApi.AccountsPlatformApiClient>();
            _stores = ServiceProvider.GetRequiredService<StoresApi.StoresApiClient>();
            _catalog = ServiceProvider.GetRequiredService<CatalogApi.CatalogApiClient>();
        }

        [Fact]
        public void SmokeTest()
        {
            // arrange
            var account = new AddAccountFake().Generate();
            var store = new AddStoreFake().Generate();
            var headers = new Metadata {new(MetaDataConstants.AccountId, account.AccountId)};

            // act
            _accounts.ProvisionAccount(account);
            _stores.AddStore(store, headers);
            var categories = new AddCategoryFake().Generate(10);
            Parallel.ForEach(categories, category => {
                _catalog.AddCategory(category, headers);
                var products = new ProductFaker().Generate(10);
                foreach (var product in products)
                {
                    product.CategoryId = category.CategoryId;
                    _catalog.AddProduct(product, headers);
                }
            });

            // assert
        }
    }
}