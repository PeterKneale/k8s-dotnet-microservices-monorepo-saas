using System.Threading;
using System.Threading.Tasks;
using Registration.Application;
using Accounts.Api;

namespace Registration.Infrastructure
{
    internal class AccountsServiceGateway : IAccountsServiceGateway
    {
        private readonly AccountsPlatformApi.AccountsPlatformApiClient _client;

        public AccountsServiceGateway(AccountsPlatformApi.AccountsPlatformApiClient client)
        {
            _client = client;
        }

        public async Task ProvisionAccountAsync(ProvisionAccountRequest request, CancellationToken token)
            => await _client.ProvisionAccountAsync(request, cancellationToken: token);
    }

}