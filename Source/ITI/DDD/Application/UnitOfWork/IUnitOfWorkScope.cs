using System;

namespace ITI.DDD.Application.UnitOfWorkBase
{
    public interface IUnitOfWorkScope : IDisposable
    {
        TParticipant Current<TParticipant>()
            where TParticipant : IDataContext, new();

        void Commit();
    }
}