using System.Threading.Tasks;
using Marten;
using Accounts.Domain;

namespace Accounts.Infrastructure.Repository
{
    public class WriteRepository : IWriteRepository
    {
        private readonly IDocumentSession _session;
        
        public WriteRepository(IDocumentStore store)
        {
            _session = store.LightweightSession();
        }

        public async Task SaveAsync(Account account)
        {
            _session.Store(account);
            await _session.SaveChangesAsync();
        }
        
        public async Task SaveAsync(User user)
        {
            _session.Store(user);
            await _session.SaveChangesAsync();
        }
    }
}