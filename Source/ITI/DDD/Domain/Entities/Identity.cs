using System;

namespace ITI.DDD.Domain.Entities
{
    public abstract class Identity : IEquatable<Identity>
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
        public Guid Guid { get; set; }

        public override string ToString()
        {
            return $"{GetType().Name}:{Guid}";
        }

        public bool Equals(Identity? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Identity)obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public static bool operator ==(Identity left, Identity right)
        {
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Identity left, Identity right)
        {
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                    return false;
                return true;
            }
            return !left.Equals(right);
        }
    }
}