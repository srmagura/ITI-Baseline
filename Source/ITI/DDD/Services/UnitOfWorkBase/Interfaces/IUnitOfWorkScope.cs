using System;
using Iti.Baseline.Core.DataContext;

namespace ITI.DDD.Services.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWorkScope : IDisposable
    {
        TParticipant Current<TParticipant>()
            where TParticipant : BaseDataContext, new();

        void Commit();
    }
}