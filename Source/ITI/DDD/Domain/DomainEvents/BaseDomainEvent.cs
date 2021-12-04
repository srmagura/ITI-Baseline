using ITI.DDD.Core;
using System;

namespace ITI.DDD.Domain.DomainEvents;

public abstract class BaseDomainEvent : IDomainEvent
{
    public DateTimeOffset DateCreatedUtc { get; } = DateTimeOffset.UtcNow;
}
