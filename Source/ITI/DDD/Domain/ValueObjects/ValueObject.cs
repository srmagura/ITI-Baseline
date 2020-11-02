using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ITI.DDD.Domain.ValueObjects
{
//[Owned]
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public bool? HasValue { get; protected set; } = true;   // EF CORE

        protected abstract IEnumerable<object?> GetAtomicValues();

        public bool Equals(ValueObject? other)
        {
            return Equals((object?)other);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            using (var thisValues = GetAtomicValues().GetEnumerator())
            {
                using (var otherValues = other.GetAtomicValues().GetEnumerator())
                {
                    while (thisValues.MoveNext() && otherValues.MoveNext())
                    {
                        if (ReferenceEquals(thisValues.Current, null) ^
                            ReferenceEquals(otherValues.Current, null))
                        {
                            return false;
                        }

                        if (thisValues.Current != null &&
                            !thisValues.Current.Equals(otherValues.Current))
                        {
                            return false;
                        }
                    }

                    return !thisValues.MoveNext() && !otherValues.MoveNext();
                }
            }
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject? lhs, ValueObject? rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ValueObject? lhs, ValueObject? rhs)
        {
            return !(lhs == rhs);
        }
    }
}