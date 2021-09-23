using AutoMapper;
using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ITI.DDD.Application
{
    public interface IDataContext : IDisposable
    {
        void Initialize(IMapper mapper, IAuditor auditor);
        void SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        List<IDomainEvent> GetAllDomainEvents();
    }
}
