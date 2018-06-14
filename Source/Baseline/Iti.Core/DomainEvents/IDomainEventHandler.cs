namespace Iti.Core.DomainEvents
{
    public interface IDomainEventHandler<in TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        void Handle(TDomainEvent ev);
    }
}