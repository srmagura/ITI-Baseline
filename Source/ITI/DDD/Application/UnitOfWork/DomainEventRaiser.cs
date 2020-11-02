using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Application.UnitOfWork
{
    internal class DomainEventRaiser : IDomainEventRaiser
    {
        public void Raise(IDomainEvent domainEvent)
        {
            var uow = UnitOfWorkImpl.CurrentUnitOfWork;
            if (uow == null)
                throw new Exception("A domain event was raised outside of a unit of work.");

            uow.RaiseDomainEvent(domainEvent);
        }
    }
}
