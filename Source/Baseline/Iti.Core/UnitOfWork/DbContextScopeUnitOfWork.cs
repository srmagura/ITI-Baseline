using System;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.DbContextScope;
using Iti.Logging;
using Iti.Utilities;

namespace Iti.Core.UnitOfWork
{
    public class DbContextScopeUnitOfWork : IUnitOfWork
    {
        private readonly IDbContextScope _dbContextScope;

        public DbContextScopeUnitOfWork(IDbContextScope dbContextScope)
        {
            _dbContextScope = dbContextScope;
        }

        public int Commit(bool waitForDomainEvents = false)
        {
            var result = _dbContextScope.SaveChanges();

            try
            {
                ProcessDomainEvents(waitForDomainEvents);
            }
            catch (Exception exc)
            {
                Log.Error("Error processing domain events!", exc);
            }

            return result;
        }

        public void Dispose()
        {
            _dbContextScope?.Dispose();
        }

        private void ProcessDomainEvents(bool waitForDomainEvents)
        {
            var dec = DomainEvents.DomainEvents.DomainEventContext;
            if (dec == null)
            {
                // NOTE: do NOT log in here... SaveChanges recurses
                // Log.Warning("Domain event raised without associated unit of work");
                return;
            }

            var events = dec.Events.ToList();
            dec.Clear();

            if (!events.HasItems())
                return;

            var task = Task.Run(() => DomainEvents.DomainEvents.HandleEvents(events));
            if (waitForDomainEvents || UnitOfWork.WaitForDomainEvents)
                task.Wait();
        }
    }
}