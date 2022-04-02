using System;

namespace Accounts.Messages
{
    public class AccountAdded : IIntegrationEvent
    {
        public AccountAdded(string tenantId,  string name)
        {
            AccountId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        public string AccountId { get; private set; }
        
        public string Name { get; private set; }
    }
}