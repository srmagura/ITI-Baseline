using ITI.DDD.Application;
using ITI.DDD.Core;

namespace TestApp.Application;

internal class NullDomainEventPublisher : IDomainEventPublisher
{
    public Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        // do nothing
        return Task.CompletedTask;
    }
}
