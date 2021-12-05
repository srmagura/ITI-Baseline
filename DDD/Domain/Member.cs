#pragma warning disable S2326 // Unused type parameters should be removed

namespace ITI.DDD.Domain;

public abstract class Member<TRoot> : Entity
    where TRoot : AggregateRoot
{ }
