namespace Management.Models
{
    public class CategoryDetailsPageModel
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string NamePath { get; set; }        
        public string? ParentCategoryId { get; set; }
    }
}
