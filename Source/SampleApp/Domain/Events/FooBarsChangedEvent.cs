using Iti.Core.DomainEvents;

namespace Domain.Events
{
    public class FooBarsChangedEvent : BaseDomainEvent
    {
        public FooBarsChangedEvent(FooId id)
        {
            FooId = id;
        }

        public FooId FooId { get; }
    }
}