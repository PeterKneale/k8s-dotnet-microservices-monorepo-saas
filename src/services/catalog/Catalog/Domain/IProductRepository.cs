using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(string productId);
        Task SaveAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<IEnumerable<Product>> ListAsync(string? categoryId = null);
    }
}