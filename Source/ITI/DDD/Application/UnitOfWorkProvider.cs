using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ITI.DDD.Core;

namespace ITI.DDD.Application;

public sealed class UnitOfWorkProvider : IUnitOfWorkProvider
{
    private readonly ILifetimeScope _lifetimeScope;

    public UnitOfWorkProvider(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }

    public IUnitOfWork? Current { get; private set; }
    private ILifetimeScope? _currentUnitOfWorkLifetimeScope;

    public IUnitOfWork Begin()
    {
        if (Current != null)
        {
            throw new NotSupportedException(
                "You are attempting to use nested units of work which is not currently supported."
            );
        }

        _currentUnitOfWorkLifetimeScope = _lifetimeScope.BeginLifetimeScope();

        Current = new UnitOfWork(
            _currentUnitOfWorkLifetimeScope,
            _currentUnitOfWorkLifetimeScope.Resolve<IDomainEventPublisher>(),
            OnUnitOfWorkDispose
        );
        return Current;
    }

    private void OnUnitOfWorkDispose()
    {
        if (_currentUnitOfWorkLifetimeScope == null)
        {
            throw new Exception($"{nameof(_currentUnitOfWorkLifetimeScope)} is unexpectedly null.");
        }

        _currentUnitOfWorkLifetimeScope.Dispose();

        Current = null;
        _currentUnitOfWorkLifetimeScope = null;
    }
}
