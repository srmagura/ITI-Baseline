using System;

namespace ITI.DDD.Services.UnitOfWorkBase
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWorkScope Begin();

        TParticipant Current<TParticipant>()
            where TParticipant : IDataContext, new();

        void OnScopeCommit();
        void OnScopeDispose();
    }
}