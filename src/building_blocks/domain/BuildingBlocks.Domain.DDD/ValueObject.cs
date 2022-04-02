using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain.DDD.Rules;

namespace BuildingBlocks.Domain.DDD
{
    public abstract class ValueObject
    {
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken()) throw new BusinessRuleBrokenException(rule);
        }
        
        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) return false;
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !EqualOperator(left, right);

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;

            var other = (ValueObject) obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode() => GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
        // Other utility methods
    }
}