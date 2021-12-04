using System;
using System.Threading.Tasks;

namespace ITI.DDD.Core;

public interface IUnitOfWork : IDisposable
{
    TDataContext GetDataContext<TDataContext>()
        where TDataContext : IDataContext;

    void RaiseDomainEvent(IDomainEvent domainEvent);

    Task CommitAsync();
}
