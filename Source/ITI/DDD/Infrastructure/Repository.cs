using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure.DataMapping;

namespace ITI.DDD.Infrastructure
{
    public abstract class Repository<TDbContext> 
        where TDbContext : IDataContext
    {
        private readonly IUnitOfWork _uow;
        protected readonly IDbEntityMapper DbMapper;

        protected Repository(IUnitOfWork uow, IDbEntityMapper dbMapper)
        {
            _uow = uow;
            DbMapper = dbMapper;
        }

        protected TDbContext Context => _uow.Current<TDbContext>();
    }
}
