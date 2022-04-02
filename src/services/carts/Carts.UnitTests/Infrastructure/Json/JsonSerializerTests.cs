using System.Linq;
using BuildingBlocks.Domain.DDD.ValueTypes;
using Carts.Domain;
using Carts.Infrastructure.Json;
using FluentAssertions;
using Xunit;

namespace Carts.UnitTests.Infrastructure.Json
{
    public class CustomJsonSerializerTests
    {
        [Fact]
        public void ToJson_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var cartId = "C";
            var currency = Currency.FromCurrencyCode("AUD");
            var cart = Cart.CreateInstance(cartId, currency);
            var productId = "P";
            var description = "x";
            var unitPrice = Money.CreateInstance(1, currency);
            var quantity = 2;
            cart.AddProduct(productId, description, quantity, unitPrice);

            // Act
            var json = cart.ToJson();
            var result = json.FromJson<Cart>();

            // Assert
            result.Should().NotBeNull();
            result!.CartId.Should().Be(cartId);
            result.TotalPrice.Should().Be(Money.CreateInstance(quantity * unitPrice.Amount, currency));
            result.Products.Should().HaveCount(1);
            result.Products.Single().ProductId.Should().Be(productId);
            result.Products.Single().UnitPrice.Should().Be(unitPrice);
            result.Products.Single().Description.Should().Be(description);
            result.Products.Single().GetPrice().Should().Be(Money.CreateInstance(quantity * unitPrice.Amount, currency));
            result.Products.Single().Quantity.Should().Be(quantity);
        }
    }
}