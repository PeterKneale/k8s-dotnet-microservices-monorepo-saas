using Ardalis.GuardClauses;

namespace Stores.Domain
{
    public class Store
    {
        public Store(string storeId, string name, string subdomain)
        {
            Guard.Against.NullOrWhiteSpace(storeId, nameof(storeId));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(subdomain, nameof(subdomain));
            StoreId = storeId;
            Name = name;
            Theme = "default";
            Subdomain = subdomain;
        }

        private Store()
        {
        }

        public string StoreId { get; private set; }
        
        public string Name { get; private set; }
        
        public string Theme { get; private set; }
        
        public string Subdomain { get; private set; }
        
        public string? Domain { get; private set; }

        public bool HasDomain => Domain != null;

        public void ChangeName(string name)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Name = name;
        }
        
        public void ChangeSubdomain(string subdomain)
        {
            Guard.Against.NullOrWhiteSpace(subdomain, nameof(subdomain));
            Subdomain = subdomain;
        }

        public void ChangeDomain(string domain)
        {
            Guard.Against.NullOrWhiteSpace(domain, nameof(domain));
            Domain = domain;
        }
        
        public void ChangeTheme(string theme)
        {
            Guard.Against.NullOrWhiteSpace(theme, nameof(theme));
            Theme = theme;
        }
    }
}