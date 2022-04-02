using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using Shopping.Application.Services;

namespace Shopping.Infrastructure.Gateways
{
    public class CatalogServiceGateway : ICatalogServiceGateway
    {
        private readonly CatalogApi.CatalogApiClient _client;
        public CatalogServiceGateway(CatalogApi.CatalogApiClient client)
        {
            _client = client;
        }

        public async Task<Product> GetProductAsync(GetProductRequest request, CancellationToken cancellationToken) =>
            await _client.GetProductAsync(request, cancellationToken: cancellationToken);
    }
}