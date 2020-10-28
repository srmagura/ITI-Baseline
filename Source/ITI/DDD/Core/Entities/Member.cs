namespace ITI.DDD.Core.Entities
{
    public abstract class Member<TRoot> : Entity 
        where TRoot : AggregateRoot { }
}