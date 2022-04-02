using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Catalog.Messages;
using FluentAssertions;
using Grpc.Core;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Search.Api;
using Xunit;
using Xunit.Abstractions;

namespace Search.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class SecurityTests
    {
        private readonly SearchApi.SearchApiClient _client;
        private readonly IBus _bus;

        public SecurityTests(Fixture api, ITestOutputHelper output)
        {
            api.OutputHelper = output;
            _client = api.GetClient();
            _bus = api.Services.GetRequiredService<IBus>();
        }

        [Fact]
        public async Task EnsureAccountIsolation()
        {
            // arrange
            var accountId1 = Guid.NewGuid().ToString();
            var accountId2 = Guid.NewGuid().ToString();
            
            var productId1 = Guid.NewGuid().ToString();
            var productId2 = Guid.NewGuid().ToString();

            var query = "apple";
            var message1 = new ProductAdded(accountId1, productId1, "Apple juice", null,"x","x","x","x");
            var message2 = new ProductAdded(accountId2, productId2, "Apple pie", null,"x","x","x","x");
            
            var headers1 = new Metadata {new(MetaDataConstants.AccountId, accountId1)};
            var headers2 = new Metadata {new(MetaDataConstants.AccountId, accountId2)};
            
            // act
            await _bus.Publish(message1);
            await _bus.Publish(message2);

            async Task AssertAccountFindsOneProduct(Metadata metadata, string productId) => await Retry.RetryAsync.ExecuteAsync(async () => {
                var response = await _client.SearchAsync(new SearchRequest {Query = query, PageOffset = 0, PageSize = 1}, metadata);
                response.Results.Should().ContainSingle(x => x.ProductId == productId);
            });

            // assert 
            await AssertAccountFindsOneProduct(headers1, productId1);
            await AssertAccountFindsOneProduct(headers2, productId2);
            
        }
    }
}