using System;
using System.Threading.Tasks;
using BuildingBlocks.Domain.DDD.ValueTypes;
using FluentAssertions;
using Grpc.Core;
using Stores.Api;
using Xunit;
using Xunit.Abstractions;

namespace Stores.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class SecurityTests
    {
        private readonly StoresApi.StoresApiClient _client;

        public SecurityTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }

        [Fact]
        public async Task CallWithoutMetaData_MetaDataRequired_ThrowsPermissionDenied()
        {
            // Arrange
            var storeId = Guid.NewGuid().ToString();
            
            // Act
            Func<Task> action = async () => {
                await _client.AddStoreAsync(new AddStoreRequest
                {
                    StoreId = storeId,
                    Name = "x"
                });
            };

            // Assert
            await action.Should()
                .ThrowAsync<RpcException>()
                .Where(x => x.StatusCode == StatusCode.PermissionDenied);
        }
    }
}