using System;
using Bogus;
using Catalog.Api;

namespace SystemTests.Fakers
{
    public class AddCategoryFake : Faker<AddCategoryRequest>
    {
        public AddCategoryFake()
        {
            RuleFor(o => o.CategoryId, f => Guid.NewGuid().ToString());
            RuleFor(o => o.Name, f => f.Commerce.ProductMaterial());
        }
    }
}