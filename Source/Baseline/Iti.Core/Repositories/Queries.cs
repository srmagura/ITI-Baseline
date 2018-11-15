using Microsoft.EntityFrameworkCore;

namespace Iti.Core.Repositories
{
    public abstract class Queries<TDbContext>
        where TDbContext : DbContext, new()
    {
        protected TDbContext Context
        {
            get
            {
                // var db = new TDbContext();
                var db = UnitOfWork.UnitOfWork.Current<TDbContext>();
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                return db;
            }
        }
    }
}