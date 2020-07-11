namespace Iti.Baseline.Core.DomainEventsBase
{
    public interface IDomainEventHandler<in TEvent>
        where TEvent : class, IDomainEvent
    {
        void Handle(TEvent domainEvent);
    }
}