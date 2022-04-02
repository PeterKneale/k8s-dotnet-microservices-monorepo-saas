using System.ComponentModel.DataAnnotations;

namespace Management.Models
{
    public class StoreDomainEditModel
    {
        [Required]
        public string Domain { get; set; }

        public bool? Validated { get; set; }
    }
}