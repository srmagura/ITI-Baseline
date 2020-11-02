
using ITI.DDD.Application.UnitOfWorkBase;

namespace ITI.DDD.Application.Repositories
{
    public abstract class Queries<TDbContext>
        where TDbContext : IDataContext, new()
    {
        private readonly IUnitOfWork _uow;

        protected Queries(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected TDbContext Context => _uow.Current<TDbContext>();
    }
}