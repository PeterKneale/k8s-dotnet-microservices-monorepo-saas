using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using FluentValidation;
using Media;
using MediatR;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Logging;
using Search.Api;
using Shopping.Api;
using Shopping.Application.Data;
using Shopping.Application.Services;

namespace Shopping.Application
{
    public static class SearchProducts
    {
        public class Validator : AbstractValidator<SearchRequest>
        {
            public Validator()
            {
                RuleFor(x => x.Query).NotEmpty().MaximumLength(250);
            }
        }

        public class Handler : IRequestHandler<SearchProductsRequest, SearchProductsResults>
        {
            private readonly ISearchServiceGateway _search;
            private readonly IMediaServiceGateway _media;
            private readonly IDataReader _reader;
            private readonly IAccountContextGetter _context;
            private readonly ILogger<Handler> _logs;

            public Handler(ISearchServiceGateway search, IMediaServiceGateway media, IDataReader reader, IAccountContextGetter context, ILogger<Handler> logs)
            {
                _search = search;
                _media = media;
                _reader = reader;
                _context = context;
                _logs = logs;
            }

            public async Task<SearchProductsResults> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
            {
                var response = await _search.SearchAsync(new SearchRequest
                {
                    Query = request.Query,
                    PageOffset = request.PageOffset,
                    PageSize = request.PageSize
                }, cancellationToken);

                var products = await LoadFromLocalStore(response);
                var galleries = await _media.GetProductsGalleries(new GetProductsGalleriesRequest
                {
                    ProductIds = {products.Select(x => x.ProductId)}
                });
                foreach (var product in products)
                {
                    product.PictureUrl.AddRange(galleries.ProductGalleries.Single(x => x.ProductId == product.ProductId).Gallery.PictureUrls);
                }
                return new SearchProductsResults
                {
                    Products = {products},
                    Total = response.Total
                };
            }

            private async Task<IEnumerable<Product>> LoadFromLocalStore(SearchResponse response)
            {
                var products = new List<Product>();
                foreach (var result in response.Results)
                {
                    var data = await _reader.GetProductById(result.ProductId);
                    if (data == null)
                    {
                        _logs.LogWarning($"Product {result.ProductId} not found in local store");
                        continue;
                    }
                    if (data.AccountId != _context.GetAccountId())
                    {
                        // hack: use multi account marten
                        throw new Exception("wrong account data");
                    }
                    var product = new Product
                    {
                        ProductId = data.ProductId,
                        Name = data.Name,
                        Description = data.Description ?? string.Empty
                    };
                    products.Add(product);
                }
                return products;
            }
        }
    }
}