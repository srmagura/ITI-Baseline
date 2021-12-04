using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ITI.DDD.Core
{
    public interface IDataContext : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        List<IDomainEvent> GetAllDomainEvents();
    }
}
