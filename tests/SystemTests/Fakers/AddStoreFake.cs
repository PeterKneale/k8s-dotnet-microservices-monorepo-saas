using System;
using Bogus;
using Stores.Api;

namespace SystemTests.Fakers
{
    public class AddStoreFake : Faker<AddStoreRequest>
    {
        public AddStoreFake()
        {
            RuleFor(o => o.StoreId, f => Guid.NewGuid().ToString());
            RuleFor(o => o.Name, f => f.Company.CompanyName());
        }
    }
}