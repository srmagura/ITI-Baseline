using Microsoft.EntityFrameworkCore;

namespace Iti.Core.Repositories
{
    public abstract class Repository<TDbContext> 
        where TDbContext : DbContext
    {
        protected TDbContext Context => UnitOfWork.UnitOfWork.Current<TDbContext>();
    }
}
