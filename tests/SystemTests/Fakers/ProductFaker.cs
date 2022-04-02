using System;
using Bogus;
using Catalog.Api;
using Google.Type;

namespace SystemTests.Fakers
{
    public class ProductFaker : Faker<AddProductRequest>
    {
        public ProductFaker()
        {
            RuleFor(o => o.ProductId, f => Guid.NewGuid().ToString());
            RuleFor(o => o.Name, f => f.Commerce.Product());
            RuleFor(o => o.Description, f => f.Commerce.ProductDescription());
            RuleFor(o => o.Price, f => new Money {DecimalValue = f.Finance.Amount(2, 300), CurrencyCode = "AUD"});
        }
    }

}