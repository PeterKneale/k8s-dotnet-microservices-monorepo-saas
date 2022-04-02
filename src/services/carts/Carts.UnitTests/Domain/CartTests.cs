using System;
using System.Linq;
using BuildingBlocks.Domain.DDD.ValueTypes;
using Carts.Domain;
using FluentAssertions;
using Xunit;

namespace Carts.UnitTests.Domain
{

    public class CartTests
    {
        private static Currency AUD = Currency.FromCurrencyCode("AUD");
        private static Currency USD = Currency.FromCurrencyCode("USD");
        private static Money AUD1 = Money.CreateInstance(1.00M, AUD);
        private static Money AUD2 = Money.CreateInstance(2.00M, AUD);
        private static Money AUD3 = Money.CreateInstance(3.00M, AUD);
        private static Money AUD4 = Money.CreateInstance(4.00M, AUD);

        [Fact]
        public void AddProduct_ItemsExist_TotalPriceIsSumOfItems()
        {
            // Arrange
            var cart = Cart.CreateInstance(Guid.NewGuid().ToString(), AUD);
            cart.AddProduct(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1, AUD1);
            cart.AddProduct(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 2, AUD2);

            // Act
            cart.AddProduct(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 3, AUD3);

            // Assert
            cart.TotalPrice.Should().Be(Money.CreateInstance(14.00M, AUD));
            cart.Products.Should().HaveCount(3);
        }

        [Fact]
        public void AddProduct_SameProductExistsWithDifferentUnitPrice_UnitPriceIsUpdated()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();
            string productId = Guid.NewGuid().ToString();
            var cart = Cart.CreateInstance(cartId, AUD);
            cart.AddProduct(productId, "x", 1, AUD1);

            // Act
            cart.AddProduct(productId, "x", 1, AUD2);

            // Assert
            cart.TotalPrice.Should().Be(AUD4);
            cart.Products.Should().HaveCount(1);
            cart.Products.Should().ContainEquivalentOf(Product.CreateInstance(productId, "x", 2, AUD2));
        }

        [Fact]
        public void AddProduct_SameProductExistsWithDifferentDescription_DescriptionIsUpdated()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();
            string productId = Guid.NewGuid().ToString();
            var cart = Cart.CreateInstance(cartId, AUD);
            var description1 = "x";
            var description2 = "y";
            cart.AddProduct(productId, description1, 1, AUD1);

            // Act
            cart.AddProduct(productId, description2, 1, AUD1);

            // Assert
            cart.Products.Should().HaveCount(1);
            cart.Products.Should().ContainEquivalentOf(Product.CreateInstance(productId, description2, 2, AUD1));
        }

        [Fact]
        public void AddProduct_SameProductExists_QuantityIsUpdated()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();
            string productId = Guid.NewGuid().ToString();
            var cart = Cart.CreateInstance(cartId, AUD);
            var quantity1 = 2;
            var quantity2 = 1;
            cart.AddProduct(productId, Guid.NewGuid().ToString(), quantity1, AUD2);

            // Act

            cart.AddProduct(productId, Guid.NewGuid().ToString(), quantity2, AUD2);

            // Assert
            cart.Products.Single().Quantity.Should().Be(quantity1 + quantity2);
        }

        [Fact]
        public void RemoveProduct_ItemExist_Total()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();
            string productId = Guid.NewGuid().ToString();

            // Act
            var cart = Cart.CreateInstance(cartId, AUD);
            cart.AddProduct(productId, Guid.NewGuid().ToString(), 3, AUD1);
            cart.RemoveProduct(productId);

            // Assert
            cart.Products.Should().HaveCount(0);
        }

        [Fact]
        public void RemoveProduct_TwoItemsExistForDifferentProducts_OneItemRemains()
        {
            // Arrange
            var cartId = Guid.NewGuid().ToString();
            string productId1 = Guid.NewGuid().ToString();
            string productId2 = Guid.NewGuid().ToString();
            var cart = Cart.CreateInstance(cartId, AUD);
            cart.AddProduct(productId1, "x", 1, AUD1);
            cart.AddProduct(productId2, "y", 1, AUD2);

            // Act
            cart.RemoveProduct(productId2);

            // Assert
            cart.Products.Should().HaveCount(1);
            cart.Products.Should().ContainEquivalentOf(Product.CreateInstance(productId1, "x", 1, AUD1));
        }

        [Fact]
        public void AddProductInAUD_CartIsUSD_Throws()
        {
            // Arrange

            // Act
            var cart = Cart.CreateInstance(Guid.NewGuid().ToString(), USD);
            Action act = () => cart.AddProduct(Guid.NewGuid().ToString(), "x", 1, AUD1);

            // Assert
            act.Should().Throw<Exception>().WithMessage("The product's unit price is a different currency to the carts");
        }

        [Fact]
        public void Empty_NoItemsExist_IsEmpty()
        {
            // Arrange
            var cart = Cart.CreateInstance(Guid.NewGuid().ToString(), AUD);

            // Act
            cart.Empty();

            // Assert
            cart.Products.Should().BeEmpty();
        }
    }
}