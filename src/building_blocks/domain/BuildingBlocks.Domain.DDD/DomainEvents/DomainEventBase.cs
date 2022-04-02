using System;

namespace BuildingBlocks.Domain.DDD.DomainEvents
{
    public abstract class DomainEventBase : IDomainEvent
    {
        protected DomainEventBase()
        {
            DomainEventId = Guid.NewGuid();
            OccuredOn = SystemClock.Now;
        }

        public Guid DomainEventId { get; }
        
        public DateTime OccuredOn { get; }
    }
}