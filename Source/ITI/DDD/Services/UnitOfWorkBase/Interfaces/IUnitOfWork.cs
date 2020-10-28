using System;

namespace ITI.DDD.Services.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWorkScope Begin();

        TParticipant Current<TParticipant>()
            where TParticipant : BaseDataContext, new();

        void OnScopeCommit();
        void OnScopeDispose();
    }
}