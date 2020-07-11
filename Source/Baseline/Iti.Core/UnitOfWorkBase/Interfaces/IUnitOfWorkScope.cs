using System;

namespace Iti.Baseline.Core.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWorkScope : IDisposable
    {
        void Commit();
    }
}