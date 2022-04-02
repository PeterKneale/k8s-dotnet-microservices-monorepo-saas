using System;

namespace Catalog.Messages
{
    public class ProductCategoryChanged : IIntegrationEvent
    {
        public ProductCategoryChanged(string accountId,string productId, string categoryId)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            CategoryId = categoryId ?? throw new ArgumentNullException(nameof(categoryId));
        }
        
        public string AccountId { get; private set; }
        
        public string ProductId { get; private set; }
        
        public string CategoryId { get; private set;}
    }
}