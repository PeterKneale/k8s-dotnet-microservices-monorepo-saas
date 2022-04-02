using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Registration.Api;
using Stores.Api;
using Accounts.Api;
using Xunit;
using Xunit.Abstractions;

namespace Registration.FunctionalTests
{
    public class RegistrationTests : IClassFixture<Fixture>
    {
        private readonly Fixture _fixture;
        private readonly RegistrationApi.RegistrationApiClient _client;

        public RegistrationTests(Fixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            fixture.OutputHelper = output;
            _client = fixture.GetClient();
        }

        [Fact]
        public async Task Registration_should_complete()
        {
            // arrange
            var reference = Guid.NewGuid().ToString();
            var random = new Random().Next(10000);
            var name = $"apple{random}";
            var email = $"apple{random}@example.com";

            _fixture.AccountsService
                .Setup(x => x.AddAccountAsync(It.IsAny<AddAccountRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            
            _fixture.AccountsService
                .Setup(x => x.AddUserAsync(It.IsAny<string>(), It.IsAny<AddUserRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _fixture.StoresService
                .Setup(x => x.AddStoreAsync(It.IsAny<string>(), It.IsAny<AddStoreRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // act
            await _client.SubmitRegistrationAsync(new SubmitRegistrationRequest
            {
                Name = name,
                Email = email,
                Reference = reference
            });

            void RegistrationShouldComplete()
            {
                _fixture.StoresService.VerifyAll();
                _fixture.AccountsService.VerifyAll();
            }

            // assert
            Retry.RetryPolicy.Execute(RegistrationShouldComplete);
        }
    }

}