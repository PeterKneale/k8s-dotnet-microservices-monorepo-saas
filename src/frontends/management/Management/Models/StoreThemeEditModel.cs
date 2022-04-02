using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Management.Models
{
    public class StoreThemeEditModel
    {
        [Required]
        public string CurrentTheme { get; set; }

        public IEnumerable<SelectListItem>? Themes{ get; set; }
    }
}