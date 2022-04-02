using System.Threading;
using System.Threading.Tasks;
using Search.Api;
using Shopping.Application.Services;

namespace Shopping.Infrastructure.Gateways
{
    public class SearchServiceGateway : ISearchServiceGateway
    {
        private readonly SearchApi.SearchApiClient _client;

        public SearchServiceGateway(SearchApi.SearchApiClient client)
        {
            _client = client;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken) =>
            await _client.SearchAsync(new SearchRequest
            {
                Query = request.Query,
                PageSize = request.PageSize,
                PageOffset = request.PageOffset
            }, cancellationToken: cancellationToken);
    }

}