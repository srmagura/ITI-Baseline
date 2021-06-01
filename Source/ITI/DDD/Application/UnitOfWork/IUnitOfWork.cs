using ITI.DDD.Core;
using System;
using System.Threading.Tasks;

namespace ITI.DDD.Application.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWorkScope Begin();

        TParticipant Current<TParticipant>()
            where TParticipant : IDataContext;

        void OnScopeCommit();
        Task OnScopeCommitAsync();
        void OnScopeDispose();

        void RaiseDomainEvent(IDomainEvent domainEvent);
    }
}