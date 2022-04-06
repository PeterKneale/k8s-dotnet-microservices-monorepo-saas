using System;
using System.Threading.Tasks;
using Accounts.Api;
using FluentAssertions;
using Grpc.Core;
using Xunit;
using Xunit.Abstractions;

namespace Accounts.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class AccountTests
    {
        private readonly AccountsPlatformApi.AccountsPlatformApiClient _client;

        public AccountTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetPlatformClient();
        }

        [Fact]
        public async Task GetAccountSummaryAsync_AccountDoesNotExist_Returns404()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();

            // Act
            Func<Task> act = async () => {
                await _client.GetAccountSummaryAsync(new GetAccountSummaryRequest
                {
                    AccountId = tenantId
                });
            };

            // Assert
            await act.Should()
                .ThrowAsync<RpcException>()
                .Where(x=>x.StatusCode == StatusCode.NotFound);
        }

        [Fact]
        public async Task ProvisionAccount_AccountDoesNotExist_ReturnsOK()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();

            // Act
            await _client.ProvisionAccountAsync(new ProvisionAccountRequest
            {
                AccountId = tenantId,
                Name = "x"
            });

            // Assert
            var tenant = await _client.GetAccountSummaryAsync(new GetAccountSummaryRequest
            {
                AccountId = tenantId
            });
            tenant.AccountId.Should().Be(tenantId);
        }
        
        [Fact]
        public async Task ListAccounts_AccountExists_AccountReturnedInList()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();

            // Act
            await _client.ProvisionAccountAsync(new ProvisionAccountRequest
            {
                AccountId = tenantId,
                Name = "x"
            });

            // Assert
            var tenants = await _client.ListAccountSummariesAsync(new ListAccountSummariesRequest());
            tenants.Accounts.Should().ContainSingle(x => x.AccountId == tenantId);
        }
    }
}