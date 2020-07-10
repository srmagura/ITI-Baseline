using System;
using Iti.Core.DateTime;

namespace Iti.Core.DomainEventsBase
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        protected BaseDomainEvent()
        {
            CreatedUtc = DateTimeService.UtcNow;
        }

        public DateTimeOffset CreatedUtc { get; }
    }
}