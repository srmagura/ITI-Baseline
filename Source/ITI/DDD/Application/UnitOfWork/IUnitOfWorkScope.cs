using System;
using System.Threading.Tasks;

namespace ITI.DDD.Application.UnitOfWork
{
    public interface IUnitOfWorkScope : IDisposable
    {
        TParticipant Current<TParticipant>()
            where TParticipant : IDataContext;

        void Commit();
        Task CommitAsync();
    }
}