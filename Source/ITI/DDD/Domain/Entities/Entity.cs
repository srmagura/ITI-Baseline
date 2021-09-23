using ITI.DDD.Core;
using System;
using System.Collections.Generic;

namespace ITI.DDD.Domain.Entities
{
    public abstract class Entity
    {
        protected Entity()
        {
            DateCreatedUtc = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset DateCreatedUtc { get; protected set; }

        public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

        protected void Raise(IDomainEvent domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }
    }
}