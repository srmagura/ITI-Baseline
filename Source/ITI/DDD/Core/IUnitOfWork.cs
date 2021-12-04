using System;
using System.Threading.Tasks;

namespace ITI.DDD.Core;

public interface IUnitOfWork : IDisposable
{
    IUnitOfWorkScope Begin();

    TParticipant Current<TParticipant>()
        where TParticipant : IDataContext;

    Task OnScopeCommitAsync();

    void RaiseDomainEvent(IDomainEvent domainEvent);
}
