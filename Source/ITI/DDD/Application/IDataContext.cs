using AutoMapper;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Application
{
    public interface IDataContext : IDisposable
    {
        void Initialize(IMapper mapper, IAuditor auditor);
        void SaveChanges();

        List<IDomainEvent> GetAllDomainEvents();
    }
}
