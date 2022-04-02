using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Search.Application.Services;

namespace Search.Application
{
    public static class SearchProducts
    {
        public record Query(string Term, string CategoryId, int PageOffset, int PageSize) : IRequest<Result>;

        public record Result(IEnumerable<SearchResult> SearchResults, long Total);

        public record SearchResult(string ProductId, string Name, string? Description);

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Term).MaximumLength(250);
                RuleFor(x => x.PageOffset).GreaterThanOrEqualTo(0);
                RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly ISearchService _service;

            public Handler(ISearchService service)
            {
                _service = service;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var documents = await _service.Search(request.Term, null, request.PageOffset, request.PageSize);
                var results = documents.Documents.Select(x => new SearchResult(x.ProductId, x.Name, x.Description));
                return new Result(results, documents.Total);
            }
        }
    }
}