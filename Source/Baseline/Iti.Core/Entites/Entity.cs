using System;
using System.Collections.Generic;
using AutoMapper;
using Iti.Core.DateTime;
using Iti.Core.DomainEventsBase;

namespace Iti.Core.Entites
{
    public abstract class Entity
    {
        protected Entity()
        {
            DateCreatedUtc = DateTimeService.UtcNow;
        }

        public DateTimeOffset DateCreatedUtc { get; protected set; }

        //

        [IgnoreMap]
        internal List<IDomainEvent> DomainEvents = null;

        protected void Raise(IDomainEvent domainEvent)
        {
            if (DomainEvents == null)
                DomainEvents = new List<IDomainEvent>();

            DomainEvents.Add(domainEvent);
        }
    }
}