using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Core
{
    public interface IDomainEventRaiser
    {
        void Raise(IDomainEvent domainEvent);
    }
}
