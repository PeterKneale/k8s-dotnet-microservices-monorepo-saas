using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nest;
using Search.Application.Services;

namespace Search.Infrastructure.Index
{
    public class IndexService : IIndexService
    {
        private readonly IElasticClient _client;
        private readonly ILogger<IndexService> _logger;
        public const string IndexName = "products";

        public IndexService(IElasticClient client, ILogger<IndexService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task UpdateProduct(ProductDocument document, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding document to index. {document}", document);
            var response = await _client.IndexDocumentAsync(document, cancellationToken);
            response.AssertSuccess();
        }

        public async Task RemoveProduct(string accountId, string productId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing document from index. account: {AccountId} product: {ProductId}", accountId, productId);
            var response = await _client.DeleteAsync<ProductDocument>(productId, ct: cancellationToken);
            response.AssertSuccess();
        }

    }

}