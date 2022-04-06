using System.Threading;
using System.Threading.Tasks;
using Accounts.Api;

namespace Registration.Application
{
    public interface IAccountsServiceGateway
    {
        Task ProvisionAccountAsync(ProvisionAccountRequest request, CancellationToken token);
    }
}