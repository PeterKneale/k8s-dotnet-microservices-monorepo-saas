namespace Management.Models
{
    public class StoreDomainViewModel
    {
        public string Domain { get; set; }
        
        public bool IsDomainValidated { get; set; }
        
        public bool HasCustomDomain => !string.IsNullOrWhiteSpace(Domain);
    }
}