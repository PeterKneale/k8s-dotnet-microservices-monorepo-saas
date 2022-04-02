using System.Linq;
using System.Threading.Tasks;
using Catalog.Application;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Grpc.Core;
using MediatR;

namespace Catalog.Api
{
    public class CatalogApiService : CatalogApi.CatalogApiBase
    {
        private readonly IMediator _mediator;

        public CatalogApiService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Empty> AddCategory(AddCategoryRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddCategory.Command(request.CategoryId, request.ParentCategoryId, request.Name));
            return new Empty();
        }
        
        public override async Task<Empty> UpdateCategory(UpdateCategoryRequest request, ServerCallContext context)
        {
            await _mediator.Send(new UpdateCategory.Command(request.CategoryId, request.Name));
            return new Empty();
        }

        public override async Task<Empty> DeleteCategory(DeleteCategoryRequest request, ServerCallContext context)
        {
            await _mediator.Send(new DeleteCategory.Command(request.CategoryId));
            return new Empty();
        }

        public override async Task<Empty> UpdateProductCategory(UpdateProductCategoryRequest request, ServerCallContext context)
        {
            await _mediator.Send(new UpdateProductCategory.Command(request.ProductId, request.CategoryId));
            return new Empty();
        }

        public override async Task<Category> GetCategory(GetCategoryRequest request, ServerCallContext context)
        {
            var category = await _mediator.Send(new GetCategory.Query(request.CategoryId));
            return Map(category);
        }

        public override async Task<CategoriesReply> ListCategories(ListCategoriesRequest request, ServerCallContext context)
        {
            var categories = await _mediator.Send(new ListCategories.Query());
            return new CategoriesReply
            {
                Categories = {categories.Select(Map)}
            };
        }

        public override async Task<Empty> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddProduct.Command(request.ProductId, request.CategoryId, request.Name, request.Description, request.Price.DecimalValue, request.Price.CurrencyCode));
            return new Empty();
        }

        public override async Task<Empty> RemoveProduct(RemoveProductRequest request, ServerCallContext context)
        {
            await _mediator.Send(new RemoveProduct.Command(request.ProductId));
            return new Empty();
        }

        public override async Task<Empty> UpdateProductDetails(UpdateProductDetailsRequest request, ServerCallContext context)
        {
            await _mediator.Send(new UpdateProductDetails.Command(request.ProductId, request.Name, request.Description));
            return new Empty();
        }
        
        public override async Task<Empty> UpdateProductPrice(UpdateProductPriceRequest request, ServerCallContext context)
        {
            await _mediator.Send(new UpdateProductPrice.Command(request.ProductId, request.Price.DecimalValue, request.Price.CurrencyCode));
            return new Empty();
        }

        public override async Task<Product> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _mediator.Send(new GetProduct.Query(request.ProductId));
            return Map(product);
        }

        public override async Task<ProductsReply> ListProducts(ListProductsRequest request, ServerCallContext context)
        {
            string? categoryId = null;
            if (!string.IsNullOrWhiteSpace(request.CategoryId))
            {
                categoryId = request.CategoryId;
            }
            var products = await _mediator.Send(new ListProducts.Query(categoryId));
            return new ProductsReply
            {
                Products = {products.Select(Map)}
            };
        }

        private static Product Map(Domain.Product product) => new Product
        {
            ProductId = product.ProductId,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Description = product.Description,
            Price = new Money
            {
                DecimalValue = product.Price,
                CurrencyCode = product.CurrencyCode
            }
        };

        private static Category Map(Domain.Category category) => new Category
        {
            CategoryId = category.CategoryId,
            ParentCategoryId = category.ParentCategoryId ?? string.Empty,
            Name = category.Name,
            Level = category.Level,
            IdPath = category.IdPath,
            NamePath = category.NamePath
        };
    }
}