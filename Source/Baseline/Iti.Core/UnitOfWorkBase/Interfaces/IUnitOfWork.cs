using System;
using Iti.Baseline.Core.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Iti.Baseline.Core.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWorkScope Begin();

        TParticipant Current<TParticipant>()
            where TParticipant : BaseDataContext;

        void OnScopeCommit();
        void OnScopeDispose();
    }
}