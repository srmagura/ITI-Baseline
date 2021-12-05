using Autofac;
using ITI.DDD.Core;
using ITI.DDD.Logging;

namespace ITI.DDD.Application.DomainEvents.Direct;

public class DirectDomainEventPublisher : IDomainEventPublisher
{
    private readonly DomainEventHandlerRegistry _registry;
    private readonly IDirectDomainEventPublisherLifetimeScopeProvider _lifetimeScopeProvider;
    private readonly ILogger _logger;

    public DirectDomainEventPublisher(
        DomainEventHandlerRegistry registry,
        IDirectDomainEventPublisherLifetimeScopeProvider lifetimeScopeProvider,
        ILogger logger
    )
    {
        _registry = registry;
        _lifetimeScopeProvider = lifetimeScopeProvider;
        _logger = logger;
    }

    private static bool _wait = false;

    public static void ShouldWaitForHandlersToComplete(bool wait)
    {
        _wait = wait;
    }

    public async Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        var tasks = new List<Task>();

        foreach (var domainEvent in domainEvents)
        {
            var handlerTypes = _registry.Registrations[domainEvent.GetType()];

            foreach (var handlerType in handlerTypes)
            {
                tasks.Add(ExecuteHandlerAsync(handlerType, domainEvent));
            }
        }

        if (_wait) await Task.WhenAll(tasks);
    }

    private async Task ExecuteHandlerAsync(Type handlerType, IDomainEvent domainEvent)
    {
        try
        {
            using var lifetimeScope = _lifetimeScopeProvider.BeginLifetimeScope();
            var handler = lifetimeScope.Resolve(handlerType);

            var handleMethod = handler.GetType()
                .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync), new[] { domainEvent.GetType() });

            if (handleMethod == null)
                throw new Exception("Could not find HandleAsync method.");

            var returnValue = handleMethod.Invoke(handler, new object[] { domainEvent });

            if (returnValue is not Task task)
                throw new Exception("HandleAsync did not return a Task.");

            await task;
        }
        catch (Exception e)
        {
            _logger.Error($"DomainEvent:{handlerType.Name}: {e.Message}", e);
        }
    }
}
