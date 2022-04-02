using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Type;
using Moq;
using Shopping.Api;
using Xunit;
using Xunit.Abstractions;

namespace Shopping.FunctionalTests
{
    public class AddProductToShoppingCartTests : IClassFixture<Fixture>
    {
        private readonly Fixture _fixture;
        private readonly ShoppingApi.ShoppingApiClient _client;

        public AddProductToShoppingCartTests(Fixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;
            fixture.OutputHelper = outputHelper;
            _client = fixture.GetClient();
        }

        [Fact]
        public async Task AddToCart_ProductExists_ProductAdded()
        {
            // arrange
            var cartId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var quantity = 1;
            var description = "x";
            var name = "x";
            var product = new Catalog.Api.Product
            {
                CategoryId = Guid.NewGuid().ToString(),
                ProductId = productId,
                Description = description,
                Name = name,
                Price = new Money{DecimalValue = 1, CurrencyCode = "AUD"}
            };
            
            _fixture.CatalogService
                .Setup(x => x.GetProductAsync(It.IsAny<Catalog.Api.GetProductRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _fixture.CartsService
                .Setup(x => x.AddProductToCartAsync(It.IsAny<Carts.Api.AddProductToCartRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            
            // act
            await _client.AddProductToShoppingCartAsync(new AddProductToShoppingCartRequest
            {
                CartId = cartId,
                Quantity = quantity,
                ProductId = productId
            });

            // assert
            _fixture.CatalogService.VerifyAll();
            _fixture.CartsService.VerifyAll();
        }
    }
}