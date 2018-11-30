using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.DbContextScope;

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

            ProcessDomainEvents(waitForDomainEvents);

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

            var task = Task.Run(() => DomainEvents.DomainEvents.HandleEvents(events));
            if (waitForDomainEvents)
                task.Wait();
        }
    }
}