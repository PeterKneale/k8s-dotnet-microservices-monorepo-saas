using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;

namespace Shopping.Application.Services
{
    public interface ICatalogServiceGateway
    {
        Task<Product> GetProductAsync(GetProductRequest request, CancellationToken cancellationToken);
    }
}