using System;

namespace ITI.DDD.Domain.Entities
{
    public abstract record Identity : IEquatable<Identity>
    {
        protected Identity()
        {
            Guid = SequentialGuid.Next();
        }

        protected Identity(Guid guid)
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