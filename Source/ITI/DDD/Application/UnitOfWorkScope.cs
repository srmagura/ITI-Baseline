using ITI.DDD.Core;
using System;
using System.Threading.Tasks;

namespace ITI.DDD.Application;

internal class UnitOfWorkScope : IUnitOfWorkScope
{
    protected readonly IUnitOfWork ParentUnitOfWork;

    internal UnitOfWorkScope(IUnitOfWork parentUnitOfWork)
    {
        ParentUnitOfWork = parentUnitOfWork;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ParentUnitOfWork.Dispose();
    }

    public TParticipant Current<TParticipant>() where TParticipant : IDataContext
    {
        return ParentUnitOfWork.Current<TParticipant>();
    }

    public Task CommitAsync()
    {
        return ParentUnitOfWork.OnScopeCommitAsync();
    }
}
