using System;

namespace Iti.Core.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWorkScope : IDisposable
    {
        void Commit();
    }
}