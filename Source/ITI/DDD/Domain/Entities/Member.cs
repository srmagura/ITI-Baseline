namespace ITI.DDD.Domain.Entities
{
    public abstract class Member<TRoot> : Entity 
        where TRoot : AggregateRoot { }
}