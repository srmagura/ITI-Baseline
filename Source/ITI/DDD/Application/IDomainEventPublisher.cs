using ITI.DDD.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.DDD.Application;

public interface IDomainEventPublisher
{
    Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents);
}
