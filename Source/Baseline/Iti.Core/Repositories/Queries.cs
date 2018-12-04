using System;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.Repositories
{
    public abstract class Queries<TDbContext>
        : IDisposable
        where TDbContext : DbContext, new()
    {
        private TDbContext _db = null;

        protected TDbContext Context
        {
            get
            {
                if (_db == null)
                {
                    _db = new TDbContext();
                    _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                }

                return _db;
            }
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}