using ITI.DDD.Core.DomainEvents;
using System;
using System.Collections.Generic;

namespace ITI.DDD.Core.Entities
{
    public abstract class Entity
    {
        protected Entity()
        {
            DateCreatedUtc = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset DateCreatedUtc { get; protected set; }

        //

        //[IgnoreMap]
        internal List<IDomainEvent> DomainEvents = null;

        protected void Raise(IDomainEvent domainEvent)
        {
            if (DomainEvents == null)
                DomainEvents = new List<IDomainEvent>();

            DomainEvents.Add(domainEvent);
        }
    }
}