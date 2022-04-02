using Ardalis.GuardClauses;

namespace Catalog.Domain
{
    public class Product
    {
        public Product(string productId,string categoryId, string name, string? description, decimal price, string currencyCode)
        {
            Guard.Against.NullOrWhiteSpace(productId, nameof(productId));
            Guard.Against.NullOrWhiteSpace(categoryId, nameof(categoryId));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NegativeOrZero(price, nameof(price));
            Guard.Against.NullOrWhiteSpace(currencyCode, nameof(currencyCode));
            ProductId = productId;
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Price = price;
            CurrencyCode = currencyCode;
        }

        private Product()
        {
        }

        public string ProductId { get; private set; }
        
        public string CategoryId { get; private set; }
        
        public string Name { get; private set; }

        public string? Description { get; private set; }

        public decimal Price { get; private set; }
        
        public string CurrencyCode { get; private set;}

        public void ChangePrice(decimal price, string currencyCode)
        {
            Guard.Against.NegativeOrZero(price, nameof(price));
            Guard.Against.NullOrWhiteSpace(currencyCode, nameof(currencyCode));
            Price = price;
            CurrencyCode = currencyCode;
        }
        
        public void ChangeCategory(string categoryId)
        {
            Guard.Against.NullOrWhiteSpace(categoryId, nameof(categoryId));
            CategoryId = categoryId;
        }
        
        public void ChangeDetails(string name, string? description)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Name = name;
            Description = description;
        }
    }
}