using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using Carts.Api;
using FluentAssertions;
using Google.Type;
using Grpc.Core;
using Xunit;
using Xunit.Abstractions;

namespace Carts.FunctionalTests
{
    [Collection(nameof(Fixture))]
    public class SecurityTests 
    {
        private readonly CartsApi.CartsApiClient _client;
        private static Metadata MetaData => new() {new Metadata.Entry(MetaDataConstants.AccountId, "1")};

        public SecurityTests(Fixture api, ITestOutputHelper outputHelper)
        {
            api.OutputHelper = outputHelper;
            _client = api.GetClient();
        }

        [Fact]
        public async Task GetBasketByIdAsync_CartDoesNotExist_Returns404()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();

            // Act
            Func<Task> act = async () => {
                await GetCartAsync(cartId);
            };

            // Assert
            var ex = await act.Should().ThrowAsync<RpcException>();
            ex.And.Status.StatusCode.Should().Be(StatusCode.NotFound);
        }

        [Fact]
        public async Task AddProductToCartAsync_CartDoesNotExist_ReturnsOK()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid().ToString();
            var description = "x";
            var quantity = 2;
            var unitPrice = new Money {DecimalValue = 2.00M, CurrencyCode = "AUD"};

            // Act
            await _client.AddProductToCartAsync(new AddProductToCartRequest
            {
                CartId = cartId,
                ProductId = productId,
                Description = description,
                Quantity = quantity,
                UnitPrice = unitPrice
            }, MetaData);

            // Assert
            var product = await GetCartAsync(cartId);
            product.CartId.Should().Be(cartId);
            product.TotalPrice.Should().BeEquivalentTo(new Money {DecimalValue = 4, CurrencyCode = "AUD"});
            product.Items.Single().ProductId.Should().Be(productId);
            product.Items.Single().Description.Should().Be(description);
            product.Items.Single().UnitPrice.Should().BeEquivalentTo(unitPrice);
            product.Items.Single().Price.Should().BeEquivalentTo(new Money {DecimalValue = 4, CurrencyCode = "AUD"});
        }


        [Fact]
        public async Task RemoveProductFromBasketAsync_CartHasItem_ReturnsOK()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();
            var productId = "1";

            // Act
            await _client.AddProductToCartAsync(new AddProductToCartRequest
            {
                CartId = cartId,
                ProductId = productId,
                Description = "x",
                Quantity = 2,
                UnitPrice = new Money {DecimalValue = 2.00M, CurrencyCode = "AUD"}
            }, MetaData);
            await _client.RemoveProductFromCartAsync(new RemoveProductFromCartRequest {CartId = cartId, ProductId = productId}, MetaData);

            // Assert
            var result = await GetCartAsync(cartId);
            result.TotalPrice.Should().BeEquivalentTo(new Money {DecimalValue = 0.00M, CurrencyCode = "AUD"});
            result.Items.Should().BeEmpty();
        }


        [Fact]
        public async Task AddProductWithOneCurrency_CartHasProductInDifferentCurrency_ThrowsBusinessRuleViolation()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();

            async Task AddProduct(string currencyCode) => await _client.AddProductToCartAsync(new AddProductToCartRequest
            {
                CartId = cartId,
                ProductId = Guid.NewGuid().ToString(),
                Description = "x",
                Quantity = 1,
                UnitPrice = new Money {DecimalValue = 1.00M, CurrencyCode = currencyCode}
            }, MetaData);

            // Act
            var currencyCode1 = "AUD";
            var currencyCode2 = "USD";
            await AddProduct(currencyCode1);
            Func<Task> act1 = async () => { await AddProduct(currencyCode1); };
            Func<Task> act2 = async () => { await AddProduct(currencyCode2); };

            // Assert
            await act1();
            await act2.Should().ThrowAsync<RpcException>()
                .WithMessage("*different currency*");
        }
        
        [Fact]
        public async Task CallWithoutMetaData_MetaDataRequired_ThrowsPermissionDenied()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();

            // Act
            Func<Task> action = async () => {
                await _client.AddProductToCartAsync(new AddProductToCartRequest
                {
                    CartId = cartId,
                    ProductId = Guid.NewGuid().ToString(),
                    Description = "x",
                    Quantity = 1,
                    UnitPrice = new Money {DecimalValue = 1.00M, CurrencyCode = "AUD"}
                });
            };

            // Assert
            await action.Should()
                .ThrowAsync<RpcException>()
                .Where(x => x.StatusCode == StatusCode.PermissionDenied);
        }
        
        private async Task<Cart> GetCartAsync(string cartId) =>
            await _client.GetCartAsync(new GetCartRequest {CartId = cartId}, MetaData);
    }
}