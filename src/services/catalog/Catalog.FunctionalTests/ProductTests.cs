using System;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Catalog.Api;
using FluentAssertions;
using Google.Type;
using Grpc.Core;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class ProductTests
    {
        private readonly CatalogApi.CatalogApiClient _client;
        private static Metadata MetaData => new() {new Metadata.Entry(MetaDataConstants.AccountId, "1")};

        public ProductTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }

        [Fact]
        public async Task GetProduct_ProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();

            // Act
            Func<Task> act = async () => {
                await _client.GetProductAsync(new GetProductRequest {ProductId = productId}, MetaData);
            };

            // Assert
            var ex = await act.Should().ThrowAsync<RpcException>();
            ex.And.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }

        [Fact]
        public async Task AddProduct_ProductDoesNotExist_ReturnsOK()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();
            var name = "x";
            var description = "x";
            var price = 2.5M;
            var currencyCode = "AUD";

            // Act
            await _client.AddCategoryAsync(new AddCategoryRequest
            {
                CategoryId = categoryId,
                ParentCategoryId = string.Empty,
                Name = "x"
            }, MetaData);
            await _client.AddProductAsync(new AddProductRequest
            {
                ProductId = productId,
                CategoryId = categoryId,
                Name = name,
                Description = description,
                Price = new Money {DecimalValue = price, CurrencyCode = currencyCode}
            }, MetaData);

            // Assert
            var product = await _client.GetProductAsync(new GetProductRequest {ProductId = productId}, MetaData);
            product.ProductId.Should().Be(productId);
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Price.DecimalValue.Should().Be(price);
            product.Price.CurrencyCode.Should().Be(currencyCode);
        }
        
        [Fact]
        public async Task UpdateProductDetails_ProductExists_ReturnsOK()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();
            var name = "x1";
            var description = "x1";
            var name2 = "x2";
            var description2 = "x2";
            var price = 2.5M;
            var currencyCode = "AUD";

            // Act
            await _client.AddCategoryAsync(new AddCategoryRequest
            {
                CategoryId = categoryId,
                ParentCategoryId = string.Empty,
                Name = "x"
            }, MetaData);
            await _client.AddProductAsync(new AddProductRequest
            {
                ProductId = productId,
                CategoryId = categoryId,
                Name = name,
                Description = description,
                Price = new Money {DecimalValue = price, CurrencyCode = currencyCode}
            }, MetaData);
                
            await _client.UpdateProductDetailsAsync(new UpdateProductDetailsRequest
            {
                ProductId = productId,
                Name = name2,
                Description = description2
            }, MetaData);

            // Assert
            var product = await _client.GetProductAsync(new GetProductRequest {ProductId = productId}, MetaData);
            product.ProductId.Should().Be(productId);
            product.Name.Should().Be(name2);
            product.Description.Should().Be(description2);
            product.Price.DecimalValue.Should().Be(price);
            product.Price.CurrencyCode.Should().Be(currencyCode);
        }

        [Fact]
        public async Task RemoveProduct_ProductExists_ReturnsOK()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var categoryId = Guid.NewGuid().ToString();

            // Act
            await _client.AddCategoryAsync(new AddCategoryRequest
            {
                CategoryId = categoryId,
                ParentCategoryId = string.Empty,
                Name = "x"
            }, MetaData);
            await _client.AddProductAsync(new AddProductRequest
            {
                ProductId = productId,
                CategoryId = categoryId,
                Name = "x",
                Description = "x",
                Price = new Money {DecimalValue = 2.5M, CurrencyCode = "AUD"}
            }, MetaData);
            await _client.RemoveProductAsync(new RemoveProductRequest {ProductId = productId}, MetaData);
            
            // Assert
            Func<Task> act = async () => {
                await _client.GetProductAsync(new GetProductRequest {ProductId = productId}, MetaData);
            };
            var ex = await act.Should().ThrowAsync<RpcException>();
            ex.And.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }
    }
}