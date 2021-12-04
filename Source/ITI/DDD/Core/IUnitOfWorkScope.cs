using System;
using System.Threading.Tasks;

namespace ITI.DDD.Core;

public interface IUnitOfWorkScope : IDisposable
{
    TParticipant Current<TParticipant>()
        where TParticipant : IDataContext;

    Task CommitAsync();
}
