using System.Threading;
using System.Threading.Tasks;
using Stores.Api;

namespace Registration.Application
{

    public interface IStoresServiceGateway
    {
        Task AddStoreAsync(string accountId, AddStoreRequest request, CancellationToken token);
    }
}