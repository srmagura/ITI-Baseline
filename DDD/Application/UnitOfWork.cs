using Autofac;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;
using ITI.DDD.Logging;

namespace ITI.DDD.Application;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ILogger _logger;
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly Action _onDispose;

    public UnitOfWork(
        ILifetimeScope lifetimeScope,
        ILogger logger,
        IDomainEventPublisher domainEventPublisher,
        Action onDispose
    )
    {
        _logger = logger;
        _lifetimeScope = lifetimeScope;
        _domainEventPublisher = domainEventPublisher;
        _onDispose = onDispose;
    }

    private readonly Dictionary<Type, IDataContext> _dataContexts = new();
    private readonly object _lock = new();

    public TDataContext GetDataContext<TDataContext>()
        where TDataContext : IDataContext
    {
        var type = typeof(TDataContext);

        if (_dataContexts.ContainsKey(type))
            return (TDataContext)_dataContexts[type];

        lock (_lock)
        {
            var dataContext = _lifetimeScope.Resolve<TDataContext>();
            _dataContexts[type] = dataContext;

            return dataContext;
        }
    }

    private readonly List<IDomainEvent> _domainEvents = new();

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    private bool _committed = false;

    public async Task CommitAsync()
    {
        if (_committed)
            throw new Exception("This unit of work has already been committed.");

        _committed = true;

        var allDomainEvents = _domainEvents.ToList();

        foreach (var dataContext in _dataContexts.Values)
        {
            await dataContext.SaveChangesAsync();
            allDomainEvents.AddRange(dataContext.GetAllDomainEvents());
        }

        try
        {
            await _domainEventPublisher.PublishAsync(allDomainEvents);
        }
        catch (Exception e)
        {
            _logger.Error("Exception occurred while publishing domain events.", e);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        foreach (var dataContext in _dataContexts.Values)
        {
            dataContext.Dispose();
        }

        _onDispose();
    }
}
