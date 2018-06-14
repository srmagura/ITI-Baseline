using EntityFrameworkCore.DbContextScope;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.UnitOfWork
{
    public static class UnitOfWork
    {
        private static readonly DbContextScopeFactory ScopeFactory = new DbContextScopeFactory();

        public static IUnitOfWork Begin()
        {
            return new DbContextScopeUnitOfWork(ScopeFactory.Create(DbContextScopeOption.ForceCreateNew));
        }

        public static TDbContext Current<TDbContext>() 
            where TDbContext : DbContext
        {
            return new AmbientDbContextLocator().Get<TDbContext>();
        }
    }
}