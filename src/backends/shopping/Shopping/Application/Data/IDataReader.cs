using System.Threading.Tasks;
using Shopping.Infrastructure.DataSources;

namespace Shopping.Application.Data
{
    public interface IDataReader
    {
        Task<StoreData?> GetStoreByDomain(string domain);
        Task<StoreData?> GetStoreBySubdomain(string subdomain);
        Task<AccountData?> GetAccountById(string accountId);
        Task<ProductData?> GetProductById(string productId);
    }
}