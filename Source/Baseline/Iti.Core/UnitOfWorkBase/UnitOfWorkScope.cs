using Iti.Core.UnitOfWorkBase.Interfaces;

namespace Iti.Core.UnitOfWorkBase
{
    public class UnitOfWorkScope : IUnitOfWorkScope
    {
        protected readonly IUnitOfWork ParentUnitOfWork;

        internal UnitOfWorkScope(IUnitOfWork parentUnitOfWork)
        {
            ParentUnitOfWork = parentUnitOfWork;
        }

        public void Dispose()
        {
            ParentUnitOfWork.OnScopeDispose();
        }

        public void Commit()
        {
            ParentUnitOfWork.OnScopeCommit();
        }
    }
}