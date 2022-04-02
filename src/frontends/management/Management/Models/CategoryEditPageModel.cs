
using System.ComponentModel.DataAnnotations;

namespace Management.Models
{
    public class CategoryEditPageModel
    {
        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
