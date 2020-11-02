using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ITI.DDD.Domain.DomainEvents
{
    public interface IDomainEvents
    {
        void Raise(IDomainEvent domainEvent);
        Task HandleAllRaisedEventsAsync();
    }
}
