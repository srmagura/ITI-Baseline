using System;

namespace ITI.DDD.Services.UnitOfWorkBase
{
    public interface IUnitOfWorkScope : IDisposable
    {
        TParticipant Current<TParticipant>()
            where TParticipant : IDataContext, new();

        void Commit();
    }
}