using Ardalis.GuardClauses;
using BuildingBlocks.Domain.DDD.ValueTypes;

namespace Carts.Domain
{
    public class Product
    {
        private Product(string productId, string description, int quantity, Money unitPrice)
        {
            Guard.Against.NullOrWhiteSpace(productId, nameof(productId));
            Guard.Against.NullOrWhiteSpace(description, nameof(description));
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Guard.Against.Null(unitPrice, nameof(unitPrice));
            ProductId = productId;
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        private Product() {}

        public static Product CreateInstance(string productId, string description, int quantity, Money unitPrice)
        {
            return new Product(productId, description, quantity, unitPrice);
        }

        public string ProductId { get; private set; }

        public int Quantity { get; private set; }

        public Money UnitPrice { get; private set; }

        public Money GetPrice() => Money.CreateInstance(UnitPrice.Amount * Quantity, UnitPrice.Currency);

        public string Description { get; private set; }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void UpdateUnitPrice(Money unitPrice)
        {
            UnitPrice = unitPrice;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }
}