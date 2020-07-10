using System;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.UnitOfWorkBase.Interfaces
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