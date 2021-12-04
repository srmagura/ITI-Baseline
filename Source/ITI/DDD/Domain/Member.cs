namespace ITI.DDD.Domain
{
    public abstract class Member<TRoot> : Entity
        where TRoot : AggregateRoot
    { }
}