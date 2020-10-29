using ITI.DDD.Services.DomainEventsBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Services
{
    public interface IDataContext : IDisposable
    {
        void Initialize(IAuditor auditor, DomainEvents domainEvents);
        void SaveChanges();
    }
}
