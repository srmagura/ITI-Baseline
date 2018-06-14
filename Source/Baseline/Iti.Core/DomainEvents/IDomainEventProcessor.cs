using System;
using System.Collections.Generic;

namespace Iti.Core.DomainEvents
{
    public interface IDomainEventProcessor
    {
        void HandleEvents(Dictionary<Type, List<Type>> handlers, List<IDomainEvent> events);
    }
}