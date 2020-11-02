using ITI.DDD.Core;
using System;

namespace ITI.DDD.Domain.DomainEvents
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        protected BaseDomainEvent()
        {
            DateCreatedUtc = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset DateCreatedUtc { get; }
    }
}