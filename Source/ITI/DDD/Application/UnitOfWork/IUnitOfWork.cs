using ITI.DDD.Core;
using System;

namespace ITI.DDD.Application.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWorkScope Begin();

        TParticipant Current<TParticipant>()
            where TParticipant : IDataContext;

        void OnScopeCommit();
        void OnScopeDispose();

        void RaiseDomainEvent(IDomainEvent domainEvent);
    }
}