using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;

namespace Accounts.Domain
{
    public class Account
    {
        public Account(string accountId, string name)
        {
            Guard.Against.NullOrWhiteSpace(accountId, nameof(accountId));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            AccountId = accountId;
            Name = name;
        }

        [ExcludeFromCodeCoverage]
        private Account()
        {
        }

        public string AccountId { get; private set; }
        
        public string Name { get; private set; }
    }
}