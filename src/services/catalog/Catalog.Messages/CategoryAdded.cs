using System;

namespace Catalog.Messages
{
    public class CategoryAdded : IIntegrationEvent
    {
        public CategoryAdded(string accountId, string categoryId, string name)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            CategoryId = categoryId ?? throw new ArgumentNullException(nameof(categoryId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        public string AccountId { get; private set; }
        
        public string CategoryId { get; private set; }
        
        public string Name { get; private set; }
    }
}