using System;
using Iti.Baseline.Core.DataContext;

namespace Iti.Baseline.Core.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWorkScope : IDisposable
    {
        TParticipant Current<TParticipant>()
            where TParticipant : BaseDataContext;

        void Commit();
    }
}