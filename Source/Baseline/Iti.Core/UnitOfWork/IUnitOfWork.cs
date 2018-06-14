using System;

namespace Iti.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}