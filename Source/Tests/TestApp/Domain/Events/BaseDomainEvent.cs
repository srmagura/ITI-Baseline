using ITI.DDD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Domain.Events;

public abstract class BaseDomainEvent : IDomainEvent
{
    public DateTimeOffset DateCreatedUtc { get; } = DateTimeOffset.UtcNow;
}
