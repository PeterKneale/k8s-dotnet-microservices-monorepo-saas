using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stores.Domain
{
    public interface IStoreRepository
    { 
        Task CreateAsync(Store store);
        Task<bool> ExistsByIdAsync(string storeId);
        Task<bool> ExistsDefaultDomainAsync(string defaultDomain);
        Task<Store?> GetByIdAsync(string storeId);
        Task<Store?> GetByDomainAsync(string domain);
        Task<Store?> GetBySubdomainAsync(string subdomain);
        Task UpdateAsync(Store store);
        Task DeleteAsync(Store store);
        
        Task<IEnumerable<Store>> ListAsync();
    }
}