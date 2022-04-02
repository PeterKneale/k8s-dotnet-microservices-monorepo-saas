using System.Collections.Generic;

namespace Management.Models
{
    public class GalleryDetailsViewModel
    {
        public string ProductId { get; set; }
        public IEnumerable<string> PictureUrls { get; set; }
    }
}