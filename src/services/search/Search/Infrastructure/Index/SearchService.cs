using System;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Microsoft.Extensions.Logging;
using Nest;
using Search.Application.Services;

namespace Search.Infrastructure.Index
{
    public class SearchService : ISearchService
    {
        private readonly IElasticClient _client;
        private readonly IAccountContextGetter _context;
        private readonly ILogger<SearchService> _logs;

        public SearchService(IElasticClient client, IAccountContextGetter context, ILogger<SearchService> logs)
        {
            _client = client;
            _context = context;
            _logs = logs;
        }

        public async Task<ProductDocuments> Search(string term, string? categoryIdPath = null, int pageOffset = 0, int pageSize = 10)
        {
            var response = await _client.SearchAsync<ProductDocument>(search => {
                return categoryIdPath == null
                    ? search
                        .Query(q => q
                            .Bool(b => b
                                .Must(MatchName(term))
                                .Filter(ByAccount())
                            )
                        )
                        .From(pageOffset)
                        .Size(pageSize)
                    : search
                        .Query(q => q
                            .Bool(b => b
                                .Must(MatchName(term))
                                .Filter(ByAccount())
                                .Filter(MatchCategoryIdPath(categoryIdPath))
                            )
                        )
                        .From(pageOffset)
                        .Size(pageSize);
            });
            return new ProductDocuments(response.Documents, response.Total);
        }
        
        private Func<QueryContainerDescriptor<ProductDocument>, QueryContainer> ByAccount() => 
            f => f.Term(p => p.Field(x => x.AccountId).Value(_context.GetAccountId()));

        private static Func<QueryContainerDescriptor<ProductDocument>, QueryContainer> MatchCategoryIdPath(string categoryIdPath) =>
            f => f.Term(p => p.Field("categoryIdPath.tree").Value(categoryIdPath));

        private static Func<QueryContainerDescriptor<ProductDocument>, QueryContainer> MatchName(string term) =>
            x => x.Match(match => match.Field(f => f.Name).Query(term));
        
        private static Func<QueryContainerDescriptor<ProductDocument>, QueryContainer> MatchDescription(string term) =>
            x => x.Match(match => match.Field(f => f.Description).Query(term));
    }
}