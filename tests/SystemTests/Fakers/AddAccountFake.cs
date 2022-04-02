using System;
using Accounts.Api;
using Bogus;

namespace SystemTests.Fakers
{
    public class AddAccountFake : Faker<AddAccountRequest>
    {
        public AddAccountFake()
        {
            RuleFor(o => o.AccountId, f => Guid.NewGuid().ToString());
            RuleFor(o => o.Name, f => f.Company.CompanyName() + " " + f.Company.CompanySuffix());
        }
    }
}