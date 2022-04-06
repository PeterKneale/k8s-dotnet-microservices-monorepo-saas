using System;
using System.Threading.Tasks;
using Accounts.Api;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using FluentAssertions;
using Grpc.Core;
using Xunit;
using Xunit.Abstractions;

namespace Accounts.FunctionalTests
{

    [Collection(nameof(Fixture))]
    public class UserTests
    {
        private readonly AccountsApplicationApi.AccountsApplicationApiClient _client;

        public UserTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetApplicationClient();
        }

        [Fact]
        public async Task GetUserByIdAsync_UserDoesNotExist_Returns404()
        {
            // Arrange
            var tenantId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var headers = new Metadata {new(MetaDataConstants.AccountId, tenantId)};

            // Act
            Func<Task> act = async () => {
                await _client.GetUserAsync(new GetUserRequest
                {
                    UserId = userId
                }, headers);
            };

            // Assert
            await act.Should()
                .ThrowAsync<RpcException>()
                .Where(x => x.StatusCode == StatusCode.NotFound);
        }

        [Fact]
        public async Task AddUser_UserDoesNotExist_ReturnsOK()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var email = $"user{Guid.NewGuid()}@example.com";
            var tenantId = Guid.NewGuid().ToString();
            var headers = new Metadata {new(MetaDataConstants.AccountId, tenantId)};

            // Act
            await _client.AddUserAsync(new AddUserRequest
            {
                UserId = userId,
                Email = email
            }, headers);

            // Assert
            var user = await _client.GetUserAsync(new GetUserRequest
            {
                UserId = userId
            }, headers);
            user.UserId.Should().Be(userId);
            user.Email.Should().Be(email);
        }

        [Fact]
        public async Task ListUsers_UserExists_UserReturnedInList()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var email = $"user{Guid.NewGuid()}@example.com";
            var tenantId = Guid.NewGuid().ToString();
            var headers = new Metadata {new(MetaDataConstants.AccountId, tenantId)};

            // Act
            await _client.AddUserAsync(new AddUserRequest
            {
                UserId = userId,
                Email = email
            }, headers);
            var users = await _client.ListUsersAsync(new ListUsersRequest(), headers);

            // Assert
            users.Users.Should().ContainSingle(x => x.UserId == userId);
        }
    }
}