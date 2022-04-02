using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounts.Domain
{
    public interface IReadRepository
    {
        Task<Account?> GetAccountByIdAsync(string accountId);
        Task<User?> GetUserByIdAsync(string userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<Account>>ListAccounts();
        Task<IEnumerable<User>>ListUsers();
    }
}