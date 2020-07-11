using System;
using Microsoft.EntityFrameworkCore;

namespace Iti.Baseline.Core.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWorkScope Begin();

        TParticipant Current<TParticipant>()
            where TParticipant : DbContext;

        void OnScopeCommit();
        void OnScopeDispose();
    }
}