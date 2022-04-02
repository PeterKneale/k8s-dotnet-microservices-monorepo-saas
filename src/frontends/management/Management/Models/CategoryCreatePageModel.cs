
using System.ComponentModel.DataAnnotations;

namespace Management.Models
{
    public class CategoryCreatePageModel
    {
        public string? ParentCategoryId { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
