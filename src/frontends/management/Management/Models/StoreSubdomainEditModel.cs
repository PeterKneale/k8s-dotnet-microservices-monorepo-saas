using System.ComponentModel.DataAnnotations;

namespace Management.Models
{
    public class StoreSubdomainEditModel
    {
        [Required]
        public string Subdomain { get; set; }
        
        public string? ParentDomain { get; set; }
    }
}