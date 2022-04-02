using System;

namespace Catalog.Messages
{
    public class ProductAdded : IIntegrationEvent
    {
        public ProductAdded(
            string accountId,
            string productId,
            string name,
            string? description,
            string categoryId,
            string categoryName,
            string categoryIdPath,
            string categoryNamePath)
        {
            AccountId = accountId;
            ProductId = productId;
            Name = name;
            Description = description;
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryIdPath = categoryIdPath;
            CategoryNamePath = categoryNamePath;
        }

        public string AccountId { get; private set; }

        public string ProductId { get; private set; }

        public string Name { get; private set; }

        public string? Description { get; private set; }

        public string CategoryId { get; private set; }

        public string CategoryName { get; private set; }

        public string CategoryIdPath { get; private set; }

        public string CategoryNamePath { get; private set; }
    }
}