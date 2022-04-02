using Management.Application;

namespace Management.Models
{
    public class CategoryListPageModel
    {
        public ListCategories.Result Categories { get; }

        public CategoryListPageModel(ListCategories.Result categories)
        {
            Categories = categories;
        }
    }
}