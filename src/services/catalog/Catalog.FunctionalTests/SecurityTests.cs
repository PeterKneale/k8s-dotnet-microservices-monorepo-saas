using System;
using System.Threading.Tasks;
using Catalog.Api;
using FluentAssertions;
using Grpc.Core;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class SecurityTests : IClassFixture<Fixture>
    {
        private readonly CatalogApi.CatalogApiClient _client;

        public SecurityTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }
        
        [Fact]
        public async Task CallWithoutMetaData_MetaDataRequired_ThrowsPermissionDenied()
        {
            // Arrange

            // Act
            Func<Task> action = async () => {
                await _client.AddCategoryAsync(new AddCategoryRequest
                {
                    CategoryId = Guid.NewGuid().ToString(),
                    ParentCategoryId = string.Empty,
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