using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Search.Application;

namespace Search.Api
{
    public class SearchApiService : SearchApi.SearchApiBase
    {
        private readonly IMediator _mediator;

        public SearchApiService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<SearchResponse> Search(SearchRequest request, ServerCallContext context)
        {
            var query = new SearchProducts.Query(request.Query, request.CategoryIdPath, request.PageOffset, request.PageSize);
            var results = await _mediator.Send(query);
            return new SearchResponse
            {
                Results =
                {
                    results.SearchResults.Select(result => new SearchResult
                    {
                        ProductId = result.ProductId,
                        Name = result.Name
                    })
                },
                Total = results.Total
            };
        }
    }
}