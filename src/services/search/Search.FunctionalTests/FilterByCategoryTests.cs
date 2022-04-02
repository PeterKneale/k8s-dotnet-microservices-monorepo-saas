using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using FluentAssertions;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Messages;
using MassTransit;
using Search.Api;
using Xunit;
using Xunit.Abstractions;

namespace Search.FunctionalTests
{

    [Collection(nameof(Fixture))]
    public class FilterByCategoryTests
    {
        private readonly SearchApi.SearchApiClient _client;
        private readonly IBus _bus;

        public FilterByCategoryTests(Fixture api, ITestOutputHelper output)
        {
            api.OutputHelper = output;
            _client = api.GetClient();
            _bus = api.Services.GetRequiredService<IBus>();
        }

        [Fact]
        public async Task Can_filter_by_a_category()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            var headers = new Metadata {new(MetaDataConstants.AccountId, accountId)};

            var categoryId1 = Guid.NewGuid().ToString();
            var categoryId2 = Guid.NewGuid().ToString();
            var categoryId3 = Guid.NewGuid().ToString();
            var productId1 = Guid.NewGuid().ToString();
            var productId2 = Guid.NewGuid().ToString();
            var productId3 = Guid.NewGuid().ToString();
            var productName1 = "apple";
            var productName2 = "banana";
            var productName3 = "carrot";

            // Act
            await _bus.Publish(new ProductAdded(accountId, productId1, productName1, productName1, categoryId1, "x",categoryId1, "x"));
            await _bus.Publish(new ProductAdded(accountId, productId2, productName2, productName2, categoryId2, "y",categoryId2, "y"));
            await _bus.Publish(new ProductAdded(accountId, productId3, productName3, productName3, categoryId3, "z",categoryId3, "z"));

            // Assert
            async Task SearchByProductNameAndFilterByCategoryId(string query, string categoryIdPath, string expectedProductId) => await Retry.RetryAsync.ExecuteAsync(async () => {
                // Search
                var response = await _client.SearchAsync(new SearchRequest
                {
                    Query = query,                  // search by product name
                    CategoryIdPath = categoryIdPath,// filtered by category
                    PageOffset = 0,
                    PageSize = 10
                }, headers);

                // Assert
                response.Results.Should().ContainSingle(x => x.ProductId == expectedProductId);
            });

            await SearchByProductNameAndFilterByCategoryId(productName1, categoryId1, productId1);
            await SearchByProductNameAndFilterByCategoryId(productName2, categoryId2, productId2);
            await SearchByProductNameAndFilterByCategoryId(productName3, categoryId3, productId3);
        }
    }
}