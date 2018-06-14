using System.Collections.Generic;

namespace Iti.Core.DomainEvents
{
    public interface IDomainEventContext
    {
        IReadOnlyCollection<IDomainEvent> Events { get; }
        void Add(IDomainEvent domainEvent);
        void Clear();
    }
}