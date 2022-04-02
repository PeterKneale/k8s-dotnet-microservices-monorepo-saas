using System;

namespace Catalog.Messages
{
    public class ProductDeleted : IIntegrationEvent
    {
        public ProductDeleted(string accountId,string productId, string name, string? description)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }
        
        public string AccountId { get; private set; }
        
        public string ProductId { get; private set; }
        
        public string Name { get; private set; }

        public string? Description { get; private set; }
    }
}