using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using BuildingBlocks.Domain.DDD.DomainEvents;
using BuildingBlocks.Domain.DDD.Rules;

namespace BuildingBlocks.Domain.DDD
{
    public abstract class Entity
    {
        private readonly IList<IDomainEvent> _domainEvents;

        protected Entity()
        {
            _domainEvents = new List<IDomainEvent>();
        }

        public ImmutableList<IDomainEvent> DomainEvents => _domainEvents.ToImmutableList();

        public void ClearDomainEvents() => _domainEvents?.Clear();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
            {
                throw new ArgumentNullException(nameof(domainEvent));
            }
            _domainEvents.Add(domainEvent);
        }

        protected void CheckRule(IBusinessRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }
            if (rule.IsBroken()) throw new BusinessRuleBrokenException(rule);
        }
    }
}