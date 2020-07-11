using System;
using Iti.Baseline.Core.Entites;

namespace Domain
{
    public class Bar : Member<Foo>
    {
        [Obsolete("Serialization use only")]
        protected Bar() { }

        public Bar(string name)
        {
            Name = name;
        }

        //
        // IDENTITY
        //

        public BarId Id { get; protected set; } = new BarId();

        //
        // ATTRIBUTES
        //

        public string Name { get; protected set; }

        //
        // OPERATIONS
        //

        internal void SetName(string name)
        {
            Name = name;
        }
    }
}