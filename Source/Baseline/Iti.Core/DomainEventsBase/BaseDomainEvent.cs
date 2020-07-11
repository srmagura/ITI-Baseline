using System;
using Iti.Baseline.Core.DateTime;

namespace Iti.Baseline.Core.DomainEventsBase
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