using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Registration.Api;
using Stores.Api;
using Accounts.Api;
using Xunit;
using Xunit.Abstractions;

namespace Registration.FunctionalTests
{
    public class RegistrationRetryTests : IClassFixture<Fixture>
    {
        private readonly Fixture _fixture;
        private readonly RegistrationApi.RegistrationApiClient _client;

        public RegistrationRetryTests(Fixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            fixture.OutputHelper = output;
            _client = fixture.GetClient();
        }

        [Fact]
        public async Task Registration_should_eventually_be_complete()
        {
            // arrange
            var reference = Guid.NewGuid().ToString();
            var random = new Random().Next(10000);
            var name = $"apple{random}";
            var email = $"apple{random}@example.com";

            // accounts has a transient failure the first try
            var sequenceAccountsService = new MockSequence();
            _fixture.AccountsService
                .InSequence(sequenceAccountsService)
                .Setup(x => x.ProvisionAccountAsync(It.IsAny<ProvisionAccountRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Transient failure"));
            _fixture.AccountsService
                .InSequence(sequenceAccountsService)
                .Setup(x => x.ProvisionAccountAsync(It.IsAny<ProvisionAccountRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _fixture.AccountsService
                .InSequence(sequenceAccountsService)
                .Setup(x => x.ProvisionAccountAsync(It.IsAny<ProvisionAccountRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // stores has a transient failure the second try
            var sequenceStoresService = new MockSequence();
            _fixture.StoresService
                .InSequence(sequenceStoresService)
                .Setup(x => x.AddStoreAsync(It.IsAny<string>(), It.IsAny<AddStoreRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _fixture.StoresService
                .InSequence(sequenceStoresService)
                .Setup(x => x.AddStoreAsync(It.IsAny<string>(), It.IsAny<AddStoreRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Transient failure"));
            _fixture.StoresService
                .InSequence(sequenceStoresService)
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
                _fixture.StoresService.Verify(ms => ms
                    .AddStoreAsync(It.IsAny<string>(), It.IsAny<AddStoreRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
                _fixture.AccountsService.Verify(ms => ms
                    .ProvisionAccountAsync(It.IsAny<ProvisionAccountRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            }

            // assert
            Retry.RetryPolicy.Execute(RegistrationShouldComplete);
        }
    }
}