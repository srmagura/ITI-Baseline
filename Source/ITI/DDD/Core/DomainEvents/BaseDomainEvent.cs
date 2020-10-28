using System;

namespace ITI.DDD.Core.DomainEvents
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