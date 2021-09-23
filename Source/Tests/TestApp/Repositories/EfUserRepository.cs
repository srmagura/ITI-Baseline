using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure.DataMapping;
using ITI.DDD.Infrastructure;
using TestApp.DataContext;
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Application.Interfaces.RepositoryInterfaces;

namespace TestApp.Repositories
{
    public class EfUserRepository : Repository<AppDataContext>, IUserRepository
    {
        public EfUserRepository(IUnitOfWork uow, IDbEntityMapper dbMapper) 
            : base(uow, dbMapper)
        {
        }

        public void Add(User user)
        {
            var dbUser = DbMapper.ToDb<DbUser>(user);
            Context.Users!.Add(dbUser);
        }
    }
}
