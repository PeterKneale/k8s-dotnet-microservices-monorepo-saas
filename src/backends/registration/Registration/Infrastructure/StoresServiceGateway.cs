using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Grpc.Core;
using Registration.Application;
using Stores.Api;

namespace Registration.Infrastructure
{
    internal class StoresServiceGateway : IStoresServiceGateway
    {
        private readonly StoresApi.StoresApiClient _client;

        public StoresServiceGateway(StoresApi.StoresApiClient client)
        {
            _client = client;
        }

        public async Task AddStoreAsync(string accountId, AddStoreRequest request, CancellationToken token)
        {
            // The store service expects calls are in the context of a account
            // that we are currently registering / provisioning
            var metadata = new Metadata
            {
                {MetaDataConstants.AccountId, accountId}
            };
            await _client.AddStoreAsync(request, metadata, cancellationToken: token);
        }
    }
}