using ITI.DDD.Core;

namespace ITI.DDD.Domain.DomainEvents
{
    public interface IDomainEventHandler<in TEvent>
        where TEvent : class, IDomainEvent
    {
        void Handle(TEvent domainEvent);
    }
}