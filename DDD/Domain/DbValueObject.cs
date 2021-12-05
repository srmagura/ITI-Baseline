using Microsoft.EntityFrameworkCore;

namespace ITI.DDD.Domain;

[Owned]
public abstract record DbValueObject
{
    public bool? HasValue { get; private init; } = true;
}
