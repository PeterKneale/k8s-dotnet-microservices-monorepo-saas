using System.ComponentModel.DataAnnotations;

namespace Management.Models
{
    public class ProductsCreatePageModel
    {
        [Required]
        public string ProductId { get; set; }
        
        [Required]
        public string CategoryId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string? Description { get; set; }
    }


}

