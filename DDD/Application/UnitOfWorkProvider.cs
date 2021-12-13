using Autofac;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;
using ITI.DDD.Logging;

namespace ITI.DDD.Application;

public sealed class UnitOfWorkProvider : IUnitOfWorkProvider
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly ILogger _logger;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public UnitOfWorkProvider(
        ILifetimeScope lifetimeScope,
        ILogger logger,
        IDomainEventPublisher domainEventPublisher
    )
    {
        _lifetimeScope = lifetimeScope;
        _logger = logger;
        _domainEventPublisher = domainEventPublisher;
    }

    public IUnitOfWork? Current { get; private set; }

    public IUnitOfWork Begin()
    {
        if (Current != null)
        {
            throw new NotSupportedException(
                "You are attempting to use nested units of work which is not currently supported. " +
                "Or you may have forgotten to dispose the last unit of work."
            );
        }

        return Current = new UnitOfWork(
            _lifetimeScope,
            _logger,
            _domainEventPublisher,
            OnUnitOfWorkDispose
        );
    }

    private void OnUnitOfWorkDispose()
    {
        Current = null;
    }
}
