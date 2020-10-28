
namespace ITI.DDD.Services.Repositories
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