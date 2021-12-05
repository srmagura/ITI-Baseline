using Autofac;
using ITI.DDD.Core;

namespace ITI.DDD.Application;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly Action _onDispose;

    public UnitOfWork(
        ILifetimeScope lifetimeScope,
        IDomainEventPublisher domainEventPublisher, 
        Action onDispose
    )
    {
        _lifetimeScope = lifetimeScope;
        _domainEventPublisher = domainEventPublisher;
        _onDispose = onDispose;
    }

    private readonly Dictionary<Type, IDataContext> _dataContexts = new();
    private readonly object _lock = new();

    public TDataContext GetDataContext<TDataContext>() where TDataContext : IDataContext
    {
        var type = typeof(TDataContext);

        if (_dataContexts.ContainsKey(type))
            return (TDataContext)_dataContexts[type];

        lock (_lock)
        {
            var dataContext = _lifetimeScope.Resolve<TDataContext>();
            _dataContexts.Add(type, dataContext);

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
        if(_committed)
            throw new Exception("This unit of work has already been committed.");

        _committed = true;

        var allDomainEvents = _domainEvents.ToList();

        foreach (var dataContext in _dataContexts.Values)
        {
            await dataContext.SaveChangesAsync();
            allDomainEvents.AddRange(dataContext.GetAllDomainEvents());
        }

        await _domainEventPublisher.PublishAsync(allDomainEvents);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _onDispose();
    }
}
