using System.Threading;
using System.Threading.Tasks;
using Search.Api;

namespace Shopping.Application.Services
{
    public interface ISearchServiceGateway
    {
        Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
    }

}