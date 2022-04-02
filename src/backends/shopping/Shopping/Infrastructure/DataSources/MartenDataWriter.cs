using System.Threading.Tasks;
using Marten;
using Shopping.Application.Data;

namespace Shopping.Infrastructure.DataSources
{
    public class MartenDataWriter : IDataWriter
    {
        private readonly IDocumentSession _session;
        
        public MartenDataWriter(IDocumentStore store)
        {
            _session = store.LightweightSession();
        }

        public async Task SaveAsync(StoreData store)
        {
            _session.Store(store);
            await _session.SaveChangesAsync();
        }
        
        public async Task SaveAsync(AccountData account)
        { 
            _session.Store(account);
            await _session.SaveChangesAsync();
        }
        
        public async Task SaveAsync(ProductData product)
        {
            _session.Store(product);
            await _session.SaveChangesAsync();
        }
    }
}