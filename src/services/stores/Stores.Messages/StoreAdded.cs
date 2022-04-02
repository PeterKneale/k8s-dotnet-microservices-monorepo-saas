using System;

namespace Stores.Messages
{
    public class StoreAdded : IIntegrationEvent
    {
        public StoreAdded(string accountId, string storeId, string name, string theme, string subdomain)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            StoreId = storeId ?? throw new ArgumentNullException(nameof(storeId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
            Subdomain = subdomain ?? throw new ArgumentNullException(nameof(subdomain));
        }

        public string AccountId { get; private set; }

        public string StoreId { get; private set; }

        public string Name { get; private set; }
        
        public string Theme { get; private set; }

        public string Subdomain { get; private set; }
    }
}