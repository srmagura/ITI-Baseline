using ITI.DDD.Core;

namespace ITI.DDD.Application.UnitOfWork
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

        public TParticipant Current<TParticipant>() where TParticipant : IDataContext, new()
        {
            return ParentUnitOfWork.Current<TParticipant>();
        }

        public void Commit()
        {
            ParentUnitOfWork.OnScopeCommit();
        }
    }
}