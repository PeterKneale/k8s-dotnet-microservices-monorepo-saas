using System;

namespace Catalog.Messages
{
    public class ProductPriceChanged : IIntegrationEvent
    {
        public ProductPriceChanged(string accountId,string productId)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            ProductId = productId;
        }
        
        public string AccountId { get; private set; }
        
        public string ProductId { get; private set; }
    }

}