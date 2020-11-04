using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;

namespace ITI.DDD.Infrastructure
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