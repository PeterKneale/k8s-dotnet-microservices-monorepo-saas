using System;

namespace BuildingBlocks.Domain.DDD.Rules
{
    public class BusinessRuleBrokenException : Exception
    {
        public BusinessRuleBrokenException(IBusinessRule brokenRule)
            : base(brokenRule.Message)
        {
        }

        public BusinessRuleBrokenException(string message)
            : base(message)
        {
            
        }
    }
}