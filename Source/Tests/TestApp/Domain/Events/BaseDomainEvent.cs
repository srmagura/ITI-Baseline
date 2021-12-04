using ITI.DDD.Core;

namespace TestApp.Domain.Events;

public abstract class BaseDomainEvent : IDomainEvent
{
    public DateTimeOffset DateCreatedUtc { get; } = DateTimeOffset.UtcNow;
}
