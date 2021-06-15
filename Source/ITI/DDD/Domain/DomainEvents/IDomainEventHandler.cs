using ITI.DDD.Core;
using System.Threading.Tasks;

namespace ITI.DDD.Domain.DomainEvents
{
    public interface IDomainEventHandler<in TEvent>
        where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent);
    }
}