using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Accounts.Domain;

namespace Accounts.Infrastructure.Repository
{
    public class ReadRepository : IReadRepository
    {
        private readonly IQuerySession _session;

        public ReadRepository(IQuerySession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Account>> ListAccounts() =>
            await _session.Query<Account>().ToListAsync();
        
        public async Task<IEnumerable<User>> ListUsers() => 
            await _session.Query<User>().ToListAsync();

        public async Task<Account?> GetAccountByIdAsync(string accountId) =>
            await _session.LoadAsync<Account>(accountId);

        public async Task<User?> GetUserByIdAsync(string userId) =>
            await _session.LoadAsync<User>(userId);

        public async Task<User?> GetUserByEmailAsync(string email) =>
            await _session.Query<User>().Where(x => x.Email == email).SingleOrDefaultAsync();
    }

}