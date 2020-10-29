using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Application
{
    public interface IDataContext : IDisposable
    {
        void Initialize(IAuditor auditor, DomainEvents domainEvents);
        void SaveChanges();
    }
}
