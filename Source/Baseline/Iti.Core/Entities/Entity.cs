using System;
using System.Collections.Generic;
using AutoMapper;
using Iti.Baseline.Core.DateTime;
using Iti.Baseline.Core.DomainEventsBase;

namespace Iti.Baseline.Core.Entities
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