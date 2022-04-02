using System.Threading;
using System.Threading.Tasks;
using Carts.Api;

namespace Shopping.Application.Services
{
    public interface ICartsServiceGateway
    {
        Task<Cart> GetCartAsync(GetCartRequest request, CancellationToken cancellationToken);
        Task AddProductToCartAsync(AddProductToCartRequest request, CancellationToken cancellationToken);
    }

}