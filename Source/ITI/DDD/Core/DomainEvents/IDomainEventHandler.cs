namespace ITI.DDD.Core.DomainEvents
{
    public interface IDomainEventHandler<in TEvent>
        where TEvent : class, IDomainEvent
    {
        void Handle(TEvent domainEvent);
    }
}