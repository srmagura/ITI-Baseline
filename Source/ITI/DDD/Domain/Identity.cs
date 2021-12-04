namespace ITI.DDD.Domain
{
    // Not abstract because it is useful to have a generic identity when
    // writing code that could be used with multiple different entities
    public record Identity : IEquatable<Identity>
    {
        public Identity()
        {
            Guid = SequentialGuid.Next();
        }

        public Identity(Guid guid)
        {
            Guid = guid;
        }

        // Setter is public for AutoMapper reasons only - do not use it
        public Guid Guid { get; init; }

        public override string ToString()
        {
            return $"{GetType().Name}:{Guid}";
        }
    }
}