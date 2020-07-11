using Iti.Baseline.Core.DomainEventsBase;

namespace Domain.Events
{
    public class FooCreatedEvent : BaseDomainEvent
    {
        public FooCreatedEvent(FooId id)
        {
            FooId = id;
        }

        public FooId FooId { get; }
    }
}