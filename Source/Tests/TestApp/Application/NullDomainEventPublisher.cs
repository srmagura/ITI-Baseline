using ITI.DDD.Application;
using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Application;

internal class NullDomainEventPublisher : IDomainEventPublisher
{
    public Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        // do nothing
        return Task.CompletedTask;
    }
}
