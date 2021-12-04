using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;

namespace ITI.DDD.Application;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ILifetimeScope _lifetimeScope;

    private readonly List<IDomainEvent> _domainEvents = new();

    public UnitOfWork(
        IDomainEventPublisher domainEventPublisher,
        ILifetimeScope lifetimeScope
    )
    {
        _domainEventPublisher = domainEventPublisher;
        _lifetimeScope = lifetimeScope;
    }

    public IUnitOfWorkScope Begin()
    {
        return new UnitOfWorkScope(this);
    }

    private readonly Dictionary<Type, IDataContext> _participants = new();
    private readonly object _lock = new();

    public TParticipant Current<TParticipant>() where TParticipant : IDataContext
    {
        var type = typeof(TParticipant);

        lock (_lock)
        {
            if (_participants.ContainsKey(type))
            {
                return (TParticipant)_participants[type];
            }

            var instance = _lifetimeScope.Resolve<TParticipant>();

            _participants.Add(type, instance);
            return instance;
        }
    }

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    private static bool WaitForDomainEvents = false;

    public static void ShouldWaitForDomainEvents(bool waitForDomainEvents)
    {
        WaitForDomainEvents = waitForDomainEvents;
    }

    async Task IUnitOfWork.OnScopeCommitAsync()
    {
        foreach (var db in _participants.Values)
        {
            await db.SaveChangesAsync();

            foreach (var domainEvent in db.GetAllDomainEvents())
            {
                RaiseDomainEvent(domainEvent);
            }
        }

        if (_domainEvents.Count != 0)
        {
            // PublishAsync is required not to throw exceptions
            var domainEventTask = _domainEventPublisher.PublishAsync(_domainEvents);

            if (WaitForDomainEvents)
                await domainEventTask;
        }

        ClearParticipants();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ClearParticipants();
    }

    private void ClearParticipants()
    {
        foreach (var p in _participants.Values)
        {
            try
            {
                p?.Dispose();
            }
            catch (Exception)
            {
                // eat it
            }
        }

        _participants.Clear();
    }
}
