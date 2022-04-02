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
        private readonly AccountsApi.AccountsApiClient _client;

        public AccountTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }

        [Fact]
        public async Task GetAccountByIdAsync_AccountDoesNotExist_Returns404()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();

            // Act
            Func<Task> act = async () => {
                await _client.GetAccountAsync(new GetAccountRequest
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
        public async Task AddAccount_AccountDoesNotExist_ReturnsOK()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();

            // Act
            await _client.AddAccountAsync(new AddAccountRequest
            {
                AccountId = tenantId,
                Name = "x"
            });

            // Assert
            var tenant = await _client.GetAccountAsync(new GetAccountRequest
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
            await _client.AddAccountAsync(new AddAccountRequest
            {
                AccountId = tenantId,
                Name = "x"
            });

            // Assert
            var tenants = await _client.ListAccountsAsync(new ListAccountRequest());
            tenants.Accounts.Should().ContainSingle(x => x.AccountId == tenantId);
        }
    }
}