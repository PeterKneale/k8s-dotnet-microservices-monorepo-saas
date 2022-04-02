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
    public class UserVerificationTests
    {
        private readonly AccountsApi.AccountsApiClient _client;

        public UserVerificationTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }

        [Fact]
        public async Task VerifyUser_UserIsUnverified_VerifiesUser()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var tenantId = Guid.NewGuid().ToString();
            var email = $"user{new Random().Next(10000)}@example.com";
            var headers = new Metadata {new(MetaDataConstants.AccountId, tenantId)};

            // Act
            await _client.AddUserAsync(new AddUserRequest
            {
                UserId = userId,
                Email = email
            }, headers);
            var token = await _client.GetEmailVerificationTokenAsync(new GetEmailVerificationTokenRequest
            {
                UserId = userId
            }, headers);
            await _client.VerifyUserAsync(new VerifyUserRequest
            {
                Email = email,
                Token = token.Token,
                Password = "secret"
            });
        }
        
        [Fact]
        public async Task VerifyUserWithInvalidToken_UserIsUnverified_Throws()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var tenantId = Guid.NewGuid().ToString();
            var email = $"user{new Random().Next(10000)}@example.com";
            var headers = new Metadata {new(MetaDataConstants.AccountId, tenantId)};

            // Act
            await _client.AddUserAsync(new AddUserRequest
            {
                UserId = userId,
                Email = email
            }, headers);
            Func<Task> act = async () => {
                await _client.VerifyUserAsync(new VerifyUserRequest
                {
                    Email = email,
                    Token = "wrong",
                    Password = "secret"
                });
            };
            
            // Assert
            var ex = await act.Should().ThrowAsync<RpcException>();
        }
    }
}