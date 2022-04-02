using Ardalis.GuardClauses;

namespace Catalog.Domain
{
    public class Category
    {
        private Category(string categoryId, string? parentCategoryIdId, string name)
        {
            Guard.Against.NullOrWhiteSpace(categoryId, nameof(categoryId));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            CategoryId = categoryId;
            ParentCategoryId = parentCategoryIdId;
            Name = name;
        }

        private Category()
        {
        }
        
        public static Category CreateInstance(string categoryId, string? parentCategoryId, string name)
        {
            return new Category(categoryId, parentCategoryId, name);
        }

        public string CategoryId { get; private set; }
        
        public string Name { get; private set; }
        
        public string? ParentCategoryId { get; private set; }
        
        // Computed by view
        public int Level { get; private set; }
        // Computed by view
        public string IdPath { get; private set; }
        // Computed by view
        public string NamePath { get; private set; }

        internal void ChangeName(string name)
        {
            Name = name;
            // namepath is invalid
        }
    }
}