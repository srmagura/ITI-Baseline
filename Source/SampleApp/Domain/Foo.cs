using System;
using System.Collections.Generic;
using Domain.Events;
using Iti.Core.DomainEvents;
using Iti.Core.Entites;
using Iti.ValueObjects;

namespace Domain
{
    public class Foo : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected Foo() { }

        public Foo(string name, List<Bar> bars, List<int> someInts, long someNumber)
        {
            Name = name;
            _bars = bars;
            _someInts = someInts;

            SomeNumber = someNumber; // Sequence.Next("OrderNumber");   ... no: don't "reach outside"

            DomainEvents.Raise(new FooCreatedEvent(Id));
        }

        //
        // IDENTITY
        //

        public FooId Id { get; protected set; } = new FooId();

        //
        // ATTRIBUTES
        //

        public string Name { get; protected set; }

        public Address Address { get; protected set; }  // NOTE: ValueObject (nullable)
        public PersonName PersonName { get; protected set; }
        public PhoneNumber PhoneNumber { get; protected set; }

        public decimal SomeMoney { get; protected set; }

        public long SomeNumber { get; protected set; }

        //
        // RELATIONSHIPS
        //

        private readonly List<Bar> _bars = new List<Bar>();
        public IReadOnlyCollection<Bar> Bars => _bars;

        private readonly List<int> _someInts = new List<int>();
        public IReadOnlyCollection<int> SomeInts => _someInts;

        private readonly List<Guid> _someGuids = new List<Guid>();
        public IReadOnlyCollection<Guid> SomeGuids => _someGuids;

        //
        // OPERATIONS
        //

        public void SetAddress(Address addr)
        {
            Address = addr;
            DomainEvents.Raise(new FooAddressChangedEvent(Id));
        }

        public void RemoveBar(string name)
        {
            _bars.RemoveAll(p => p.Name == name);
            DomainEvents.Raise(new FooBarsChangedEvent(Id));
        }

        public void AddBar(string name)
        {
            _bars.Add(new Bar(name));
            DomainEvents.Raise(new FooBarsChangedEvent(Id));
        }

        public void SetName(string newName)
        {
            Name = newName;
        }

        public void SetAllBarNames(string name)
        {
            foreach (var bar in _bars)
            {
                bar.SetName(name);
            }
            DomainEvents.Raise(new FooBarsChangedEvent(Id));
        }
    }
}