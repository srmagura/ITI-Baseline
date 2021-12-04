using ITI.DDD.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.DDD.Domain.DomainEvents
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents);
    }
}
