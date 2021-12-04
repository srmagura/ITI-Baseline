using System;

namespace ITI.DDD.Core;

public interface IDomainEvent
{
    public DateTimeOffset DateCreatedUtc { get; }
}
