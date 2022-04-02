using System;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using FluentAssertions;
using Grpc.Core;
using Stores.Api;
using Xunit;
using Xunit.Abstractions;

namespace Stores.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class BasicTests
    {
        private readonly StoresApi.StoresApiClient _client;

        public BasicTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }

        [Fact]
        public async Task GetStoreByIdAsync_StoreDoesNotExist_Returns404()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            var storeId = Guid.NewGuid().ToString();
            var metadata = new Metadata {{MetaDataConstants.AccountId, accountId}};

            // Act
            Func<Task> act = async () => {
                await _client.GetStoreAsync(new GetStoreRequest
                {
                    StoreId = storeId
                }, metadata);
            };

            // Assert
            await act.Should()
                .ThrowAsync<RpcException>()
                .Where(x => x.StatusCode == StatusCode.NotFound);
        }

        [Fact]
        public async Task AddStore_StoreDoesNotExist_ReturnsOK()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            var storeId = Guid.NewGuid().ToString();
            var metadata = new Metadata {{MetaDataConstants.AccountId, accountId}};

            // Act
            await _client.AddStoreAsync(new AddStoreRequest
            {
                StoreId = storeId,
                Name = "x"
            }, metadata);

            // Assert
            var store = await _client.GetStoreAsync(new GetStoreRequest
            {
                StoreId = storeId
            }, metadata);
            store.StoreId.Should().Be(storeId);
        }

        [Fact]
        public async Task ListStores_StoreExists_StoreReturnedInList()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            var storeId = Guid.NewGuid().ToString();
            var metadata = new Metadata {{MetaDataConstants.AccountId, accountId}};

            // Act
            await _client.AddStoreAsync(new AddStoreRequest
            {
                StoreId = storeId,
                Name = "x"
            }, metadata);

            // Assert
            var stores = await _client.ListStoresAsync(new ListStoresRequest());
            stores.Stores.Should().ContainSingle(x => x.StoreId == storeId);
        }
    }
}