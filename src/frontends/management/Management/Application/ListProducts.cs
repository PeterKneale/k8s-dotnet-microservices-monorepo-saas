using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Api;
using FluentValidation;
using Media;
using MediatR;

namespace Management.Application
{
    public static class ListProducts
    {
        public record Query(string? CategoryId = null) : IRequest<Result>;

        public class Result
        {
            public string? CategoryId { get; init; }
            public IEnumerable<ProductViewModel> Products { get; init; }

        }
        
        public class ProductViewModel
        {
            public string ProductId { get; init; }
            public string Name { get; init; }
            public string Description { get; init; }
            public string Price { get; init; }
            public string? PictureUrl { get; init; }
        }

        public class Validator : AbstractValidator<Query>
        {
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private const int BatchSize = 100;
            private readonly MediaApi.MediaApiClient _media;

            private readonly CatalogApi.CatalogApiClient _catalog;

            public Handler(CatalogApi.CatalogApiClient catalog, MediaApi.MediaApiClient media)
            {
                _catalog = catalog;
                _media = media;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await GetProducts(request.CategoryId, cancellationToken);
                var galleries = await GetProductGalleries(cancellationToken, products);
                var productViewModels = BuildProductViewModels(products, galleries);
                return new Result { Products = productViewModels, CategoryId = request.CategoryId };
            }

            private async Task<IEnumerable<Product>> GetProducts(string? categoryId, CancellationToken cancellationToken)
            {
                var request = new ListProductsRequest
                {
                    CategoryId = categoryId ?? string.Empty
                };
                var reply = await _catalog.ListProductsAsync(request, cancellationToken: cancellationToken);
                return reply.Products;
            }

            private async Task<IEnumerable<ProductGallery>> GetProductGalleries(CancellationToken cancellationToken, IEnumerable<Product> products)
            {
                var replies = new List<GetProductsGalleriesReply>();
                foreach (var batch in products.Select(x => x.ProductId).Batch(BatchSize))
                {
                    var request = new GetProductsGalleriesRequest { ProductIds = { batch } };
                    var reply = await _media.GetProductsGalleriesAsync(request, cancellationToken: cancellationToken);
                    replies.Add(reply);
                }
                return replies.SelectMany(x => x.ProductGalleries);
            }

            private IEnumerable<ProductViewModel> BuildProductViewModels(IEnumerable<Product> products, IEnumerable<ProductGallery> galleries)
            {
                var results = new List<ProductViewModel>();
                var ids = products.Select(x => x.ProductId);
                foreach (var id in ids)
                {
                    var product = products.Single(x => x.ProductId == id);
                    var gallery = galleries.Single(x => x.ProductId == id);
                    results.Add(MapToProductViewModel(product, gallery.Gallery));
                }
                return results;
            }

            private static ProductViewModel MapToProductViewModel(Product product, Gallery gallery) => new()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = $"{product.Price.DecimalValue.ToString("C")} {product.Price.CurrencyCode}",
                PictureUrl = gallery.PictureUrls.FirstOrDefault()
            };
        }
    }
}