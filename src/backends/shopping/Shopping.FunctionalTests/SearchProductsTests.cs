using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Messages;
using FluentAssertions;
using Grpc.Core;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Search.Api;
using Shopping.Api;
using Stores.Messages;
using Accounts.Messages;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Xunit;
using Xunit.Abstractions;

namespace Shopping.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class SearchProductsTests
    {
        private readonly Fixture _fixture;
        private readonly ShoppingApi.ShoppingApiClient _client;
        private readonly IBus _bus;

        public SearchProductsTests(Fixture fixture, ITestOutputHelper output)
        {
            fixture.OutputHelper = output;
            _fixture = fixture;
            _client = fixture.GetClient();
            _bus = fixture.Services.GetRequiredService<IBus>();
        }

        [Fact]
        public async Task SearchProducts_ProductsExist_ReturnsOk()
        {
            // Arrange
            var name = "apple";

            var accountId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var headers = new Metadata {new(MetaDataConstants.AccountId, accountId)};
            await _bus.Publish(new ProductAdded(accountId, productId, "x", null, "x", "x", "x", "x"));

            _fixture.SearchService
                .Setup(x => x.SearchAsync(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SearchResponse
                {
                    Results = {new List<SearchResult> {new() {ProductId = productId}}},
                    Total = 1
                });

            // Act
            async Task SearchProducts()
            {
                var store = await _client.SearchProductsAsync(new SearchProductsRequest
                {
                    Query = name
                }, headers);
                store.Products.Should().HaveCount(1);
                store.Total.Should().Be(1);
            }

            // Assert
            await Retry.RetryAsync.ExecuteAsync(async () => {
                await SearchProducts();
            });
        }

        [Fact]
        public async Task SearchProducts_ProductDoesNotExist_ReturnsResultsWithoutItem()
        {
            // Arrange
            var name = "apple";
            var accountId = Guid.NewGuid().ToString();
            var headers = new Metadata {new(MetaDataConstants.AccountId, accountId)};

            _fixture.SearchService
                .Setup(x => x.SearchAsync(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SearchResponse
                {
                    Results =
                    {
                        new List<SearchResult>
                        {
                            new() {ProductId = Guid.NewGuid().ToString(), Name = "x"}
                        }
                    },
                    Total = 1
                });

            // Act
            async Task SearchProducts()
            {
                var store = await _client.SearchProductsAsync(new SearchProductsRequest
                {
                    Query = name
                }, headers);
                store.Products.Should().HaveCount(0);
                store.Total.Should().Be(1);
            }

            // Assert
            await Retry.RetryAsync.ExecuteAsync(async () => {
                await SearchProducts();
            });
        }
    }
}