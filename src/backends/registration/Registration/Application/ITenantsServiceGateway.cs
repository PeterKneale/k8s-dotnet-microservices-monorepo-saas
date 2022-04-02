using System.Threading;
using System.Threading.Tasks;
using Accounts.Api;

namespace Registration.Application
{
    public interface IAccountsServiceGateway
    {
        Task AddAccountAsync(AddAccountRequest request, CancellationToken token);
        Task AddUserAsync(string accountId, AddUserRequest request, CancellationToken token);
    }
}