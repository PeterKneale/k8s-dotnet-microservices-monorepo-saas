using System.Threading;
using System.Threading.Tasks;
using Carts.Api;
using Shopping.Application.Services;

namespace Shopping.Infrastructure.Gateways
{
    public class CartsServiceGateway : ICartsServiceGateway
    {
        private readonly CartsApi.CartsApiClient _client;

        public CartsServiceGateway(CartsApi.CartsApiClient client)
        {
            _client = client;
        }

        public async Task<Cart> GetCartAsync(GetCartRequest request, CancellationToken cancellationToken) =>
            await _client.GetCartAsync(request, cancellationToken: cancellationToken);

        public async Task AddProductToCartAsync(AddProductToCartRequest request, CancellationToken cancellationToken) =>
            await _client.AddProductToCartAsync(request, cancellationToken: cancellationToken);
    }

}