using System;

namespace Iti.Core.DomainEvents
{
    public interface IDomainEvent
    {
        DateTimeOffset CreatedUtc { get; }
    }
}