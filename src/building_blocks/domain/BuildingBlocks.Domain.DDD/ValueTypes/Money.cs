using System.Collections.Generic;

namespace BuildingBlocks.Domain.DDD.ValueTypes
{
    public class Money : ValueObject
    {
        private Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }
        
        private Money() {}
        
        public static Money CreateInstance(decimal amount, Currency currency)
        {
            return new Money(amount, currency);
        }

        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}