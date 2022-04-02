using System;
using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Api;
using Stores.Messages;
using Accounts.Messages;
using Xunit;
using Xunit.Abstractions;

namespace Shopping.FunctionalTests
{

    [Collection(nameof(Fixture))]
    public class GetStoreTests
    {
        private readonly ShoppingApi.ShoppingApiClient _client;
        private readonly IBus _bus;

        public GetStoreTests(Fixture fixture, ITestOutputHelper outputHelper)
        {
            fixture.OutputHelper = outputHelper;
            _client = fixture.GetClient();
            _bus = fixture.Services.GetRequiredService<IBus>();
        }

        [Fact]
        public async Task GetStore_StoreDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var domain = Guid.NewGuid().ToString();

            // Act
            Func<Task> act = async () => {
                await _client.GetStoreAsync(new GetStoreRequest {Domain = domain});
            };

            // Assert
            var ex = await act.Should().ThrowAsync<RpcException>();
            ex.And.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }

        [Fact]
        public async Task GetStoreBySubdomain_StoreExists_ReturnsOk()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            var storeId = Guid.NewGuid().ToString();
            var random = new Random().Next(10000);
            var subdomain = $"example{random}";
            
            // Act
            await _bus.Publish(new AccountAdded(accountId, "ACME Incorporated"));
            await _bus.Publish(new StoreAdded(accountId, storeId, "ACME Products", "x", subdomain));

            // Assert
            await Retry.RetryAsync.ExecuteAsync(async () => {
                await _client.GetStoreAsync(new GetStoreRequest {Domain = subdomain });
            });
        }
        
        [Fact]
        public async Task GetStoreByDomain_StoreHasDomain_ReturnsOk()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            var storeId = Guid.NewGuid().ToString();
            var random = new Random().Next(10000);
            var subdomain = $"example{random}";
            var domain = $"example{random}.com";

            // Act
            await _bus.Publish(new AccountAdded(accountId, "ACME Incorporated"));
            await _bus.Publish(new StoreAdded(accountId, storeId, "ACME Products", "x", subdomain));
            await _bus.Publish(new StoreUpdated(accountId, storeId, "ACME Products", "x", subdomain, domain));

            // Assert
            await Retry.RetryAsync.ExecuteAsync(async () => {
                await _client.GetStoreAsync(new GetStoreRequest {Domain = domain});
            });
        }
    }
}