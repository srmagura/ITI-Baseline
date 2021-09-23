using ITI.DDD.Core;
using System.Threading.Tasks;

namespace ITI.DDD.Domain.DomainEvents
{
    public interface IDomainEvents
    {
        void Raise(IDomainEvent domainEvent);
        Task HandleAllRaisedEventsAsync();
    }
}
