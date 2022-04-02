using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(string categoryId);
        Task SaveAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<IEnumerable<Category>> ListAsync();
    }
}