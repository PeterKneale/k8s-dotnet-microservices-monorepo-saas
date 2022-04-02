using Microsoft.Extensions.Logging;
using Nest;
using Search.Application.Services;

namespace Search.Infrastructure.Index
{
    public class IndexManagementService : IIndexManagementService
    {
        private readonly IElasticClient _client;
        private readonly ILogger<IndexService> _logger;
        public const string IndexName = "products";

        public IndexManagementService(IElasticClient client, ILogger<IndexService> logger)
        {
            _client = client;
            _logger = logger;
        }

        /// <summary>
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/analysis-pathhierarchy-tokenizer.html
        /// </summary>
        private void CreateIndex()
        {
            string tokenizerName = "path_tokenizer";
            string analyzerName = "path_analyzer";
            var delimiter = ',';
            var response = _client.Indices.Create(IndexName, c => c
                .Settings(s => s
                    .NumberOfShards(1)
                    .NumberOfReplicas(1)
                    .Analysis(an => an
                        .Tokenizers(tokenizers => tokenizers
                            .PathHierarchy(tokenizerName, _ => new PathHierarchyTokenizer
                            {
                                Delimiter = delimiter
                            }))
                        .Analyzers(descriptor => descriptor
                            .Custom(analyzerName, _ => new CustomAnalyzer
                            {
                                Tokenizer = tokenizerName
                            }))
                    )
                )
                .Map<ProductDocument>(m => m
                    .AutoMap()
                    .Properties(props => props
                        .Keyword(x=>x.Name(n=>n.AccountId))
                        .Keyword(x=>x.Name(n=>n.ProductId))
                        .Keyword(x=>x.Name(n=>n.CategoryId))
                        .Keyword(x => x
                            .Name(n => n.CategoryIdPath)
                            .Fields(ff => ff
                                .Text(tt => tt.Analyzer(analyzerName).Name("tree"))
                            )
                        )
                    )
                ));
            response.AssertSuccess();
        }

        private void DeleteIndex()
        {
            _logger.LogInformation("Deleting index");
            var response = _client.Indices.Delete(IndexName);
            response.AssertSuccess();
        }

        public void EnsureIndexExists()
        {
            if (!CheckIndexExists())
            {
                CreateIndex();
            }
        }

        public void ReCreateIndex()
        {
            if (CheckIndexExists())
            {
                DeleteIndex();
            }
            CreateIndex();
        }

        private bool CheckIndexExists()
        {
            _logger.LogInformation("Checking index exists..");
            var response = _client.Indices.Exists(IndexName);
            return response.Exists;
        }
    }
}