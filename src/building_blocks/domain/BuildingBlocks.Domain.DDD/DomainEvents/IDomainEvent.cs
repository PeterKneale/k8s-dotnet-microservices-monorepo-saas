using System;

namespace BuildingBlocks.Domain.DDD.DomainEvents
{
    public interface IDomainEvent
    {
        Guid DomainEventId { get; }
        DateTime OccuredOn { get; }
    }
}