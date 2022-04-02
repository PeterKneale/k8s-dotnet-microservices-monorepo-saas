using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Registration.Application;
using Accounts.Api;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;

namespace Registration.Infrastructure
{
    internal class AccountsServiceGateway : IAccountsServiceGateway
    {
        private readonly AccountsApi.AccountsApiClient _client;

        public AccountsServiceGateway(AccountsApi.AccountsApiClient client)
        {
            _client = client;
        }

        public async Task AddAccountAsync(AddAccountRequest request, CancellationToken token) =>
            await _client.AddAccountAsync(request, cancellationToken: token);
        
        public async Task AddUserAsync(string accountId, AddUserRequest request, CancellationToken token)
        { 
            // The  service expects calls are in the context of a account
            // that we are currently registering / provisioning
            var metadata = new Metadata
            {
                {MetaDataConstants.AccountId, accountId}
            };
            await _client.AddUserAsync(request, metadata, cancellationToken: token);
        }
            
    }

}