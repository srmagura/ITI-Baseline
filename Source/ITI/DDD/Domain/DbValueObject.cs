using Microsoft.EntityFrameworkCore;

namespace ITI.DDD.Domain.ValueObjects
{
    [Owned]
    public abstract record DbValueObject
    {
        public bool? HasValue { get; private init; } = true;
    }
}