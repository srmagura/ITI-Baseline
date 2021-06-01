using ITI.DDD.Core;
using System.Threading.Tasks;

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

        public TParticipant Current<TParticipant>() where TParticipant : IDataContext
        {
            return ParentUnitOfWork.Current<TParticipant>();
        }

        public void Commit()
        {
            ParentUnitOfWork.OnScopeCommit();
        }

        public async Task CommitAsync()
        {
            await ParentUnitOfWork.OnScopeCommitAsync();
        }
    }
}