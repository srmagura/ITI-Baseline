using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using TestApp.Domain;
using TestApp.Domain.Events;
using TestApp.Domain.ValueObjects;

namespace TestApp.Domain
{
    public class Customer : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected Customer() { }

        public Customer(string name, //List<Bar> bars, 
            List<int> someInts, long someNumber)
        {
            Name = name;
            //_bars = bars;
            //_someInts = someInts;
            SomeNumber = someNumber;

            Raise(new CustomerAddedEvent(Id));
        }

        //
        // IDENTITY
        //

        public CustomerId Id { get; protected set; } = new CustomerId();

        //
        // ATTRIBUTES
        //

        public string Name { get; protected set; }

        public SimpleAddress? Address { get; set; }
        public SimplePersonName? ContactName { get; set; }
        public PhoneNumber? ContactPhone { get; set; }

        public decimal SomeMoney { get; protected set; }

        public long SomeNumber { get; protected set; }

        //
        // RELATIONSHIPS
        //

        //private readonly List<Bar> _bars = new List<Bar>();
        //public IReadOnlyCollection<Bar> Bars => _bars;

        //private readonly List<int> _someInts = new List<int>();
        //public IReadOnlyCollection<int> SomeInts => _someInts;

        //private readonly List<Guid> _someGuids = new List<Guid>();
        //public IReadOnlyCollection<Guid> SomeGuids => _someGuids;

        //
        // OPERATIONS
        //

        public void SetAddress(SimpleAddress address)
        {
            Address = address;
            Raise(new CustomerAddressChangedEvent(Id));
        }

        //public void RemoveBar(string name)
        //{
        //    _bars.RemoveAll(p => p.Name == name);
        //    Raise(new FooBarsChangedEvent(Id));
        //}

        //public void AddBar(string name)
        //{
        //    _bars.Add(new Bar(name));
        //    Raise(new FooBarsChangedEvent(Id));
        //}

        public void SetName(string newName)
        {
            Name = newName;
        }

        //public void SetAllBarNames(string name)
        //{
        //    var i = 1;
        //    foreach (var bar in _bars)
        //    {
        //        bar.SetName($"{name} {i}");
        //        i++;
        //    }
        //    Raise(new FooBarsChangedEvent(Id));
        //}
    }
}