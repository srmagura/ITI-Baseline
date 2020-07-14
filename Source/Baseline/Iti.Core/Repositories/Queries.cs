using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;

namespace Iti.Baseline.Core.Repositories
{
    public abstract class Queries<TDbContext>
        where TDbContext : BaseDataContext, new()
    {
        private readonly IUnitOfWork _uow;

        protected Queries(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected TDbContext Context => _uow.Current<TDbContext>();
    }
}