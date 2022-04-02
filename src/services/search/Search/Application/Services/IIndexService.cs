using System.Threading;
using System.Threading.Tasks;

namespace Search.Application.Services
{
    public interface IIndexService
    {
        Task UpdateProduct(ProductDocument document, CancellationToken cancellationToken = default);
        Task RemoveProduct(string accountId, string productId, CancellationToken cancellationToken = default);
    }

}