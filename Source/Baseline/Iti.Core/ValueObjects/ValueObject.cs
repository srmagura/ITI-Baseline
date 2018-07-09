using System;
using System.Collections.Generic;
using System.Reflection;
using Iti.Core.DTOs;
using Iti.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.ValueObjects
{
    [Owned]
    public abstract class ValueObject<T> : IEquatable<T>, IDto, IValueObject
        where T : ValueObject<T>
    {
        public long Id { get; protected set; } // MAKE EFCORE HAPPY... SIGH

        public abstract bool HasValue();

        protected bool HasAnyValue(params string[] fields)
        {
            foreach (var f in fields)
            {
                if (f.HasValue())
                    return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            T other = obj as T;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            IEnumerable<FieldInfo> fields = GetFields();

            int startValue = 17;
            int multiplier = 59;

            int hashCode = startValue;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            return hashCode;
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
                return false;

            var t = GetType();
            var otherType = other.GetType();

            if (t != otherType)
            {
                if (!t.IsAssignableFrom(otherType) && !otherType.IsAssignableFrom(t))
                    return false;
            }

            var fields = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            foreach (var field in fields)
            {
                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (!value1.Equals(value2))
                    return false;
            }

            return true;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var t = GetType();

            var fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                if (t != null)
                {
                    fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                    t = t.BaseType;
                }
            }

            return fields;
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            if ((object)x == null) return (object)y == null;

            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
        {
            return !(x == y);
        }
    }
}