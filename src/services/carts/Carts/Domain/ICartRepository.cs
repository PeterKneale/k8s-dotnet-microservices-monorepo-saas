using System.Threading.Tasks;

namespace Carts.Domain
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(string cartId);
        Task SaveAsync(Cart cart);
        Task UpdateAsync(Cart cart);
    }
}