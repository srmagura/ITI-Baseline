using Iti.Baseline.Core.ValueObjects;
using Iti.Baseline.Utilities;

namespace Domain
{
    public class ValueParent : ValueObject<ValueParent>
    {
        protected ValueParent() { }

        public ValueParent(string value, ValueChild child)
        {
            ParentValue = value;
            Child = child;
        }

        public string ParentValue { get; protected set; }
        public ValueChild Child { get; protected set; }

        public override bool HasValue()
        {
            return ParentValue.HasValue();
        }
    }

    public class ValueChild : ValueObject<ValueChild>
    {
        protected ValueChild() { }

        public ValueChild(string value)
        {
            ChildValue = value;
        }

        public string ChildValue { get; protected set; }

        public override bool HasValue()
        {
            return ChildValue.HasValue();
        }
    }
}