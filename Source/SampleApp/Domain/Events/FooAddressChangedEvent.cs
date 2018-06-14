using Iti.Core.DomainEvents;

namespace Domain.Events
{
    public class FooAddressChangedEvent : BaseDomainEvent
    {
        public FooAddressChangedEvent(FooId id)
        {
            FooId = id;
        }

        public FooId FooId { get; }
    }
}