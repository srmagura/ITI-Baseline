using Autofac;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
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