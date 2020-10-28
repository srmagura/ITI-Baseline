using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;

namespace ITI.DDD.Services.UnitOfWorkBase
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

        public TParticipant Current<TParticipant>() where TParticipant : BaseDataContext, new()
        {
            return ParentUnitOfWork.Current<TParticipant>();
        }

        public void Commit()
        {
            ParentUnitOfWork.OnScopeCommit();
        }
    }
}