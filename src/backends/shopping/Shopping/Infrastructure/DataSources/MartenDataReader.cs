using System.Threading.Tasks;
using Marten;
using Shopping.Application.Data;
using Shopping.Infrastructure.DataSources.Queries;

namespace Shopping.Infrastructure.DataSources
{
    public class MartenDataReader : IDataReader
    {
        private readonly IQuerySession _session;

        public MartenDataReader(IQuerySession session)
        {
            _session = session;
        }

        public async Task<StoreData?> GetStoreByDomain(string domain) =>
            await _session.QueryAsync(new GetStoreByDomainQuery {Domain = domain});
        
        public async Task<StoreData?> GetStoreBySubdomain(string subdomain) =>
            await _session.QueryAsync(new GetStoreBySubdomainQuery {Subdomain = subdomain});

        public async Task<AccountData?> GetAccountById(string accountId) =>
            await _session.LoadAsync<AccountData>(accountId);
        
        public async Task<ProductData?> GetProductById(string productId) =>
            await _session.LoadAsync<ProductData>(productId);
    }
}