using ITI.DDD.Core;

namespace ITI.DDD.Infrastructure
{
    public abstract class Queries<TDbContext>
        where TDbContext : IDataContext
    {
        private readonly IUnitOfWork _uow;

        protected Queries(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected TDbContext Context => _uow.Current<TDbContext>();
    }
}