using ITI.DDD.Core;
using System;
using System.Collections.Generic;

namespace ITI.DDD.Domain.Entities;

/// <summary>
/// Should not be used directly in applications. Use AggregateRoot or Member instead.
/// </summary>
public abstract class Entity
{
    public DateTimeOffset DateCreatedUtc { get; protected set; } = DateTimeOffset.UtcNow;

    public List<IDomainEvent> DomainEvents { get; } = new();

    protected void Raise(IDomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }
}
