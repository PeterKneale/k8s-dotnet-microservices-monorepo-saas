using System;
using System.Collections.Generic;

namespace BuildingBlocks.Domain.DDD.ValueTypes
{
    public class Currency : ValueObject
    {
        private Currency(string currencyCode)
        {
            CurrencyCode = currencyCode;
        }
        
        private Currency() {}

        public static Currency FromCurrencyCode(string currencyCode)
        {
            return new Currency(currencyCode);
        }

        public string CurrencyCode { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CurrencyCode;
        }
    }
}