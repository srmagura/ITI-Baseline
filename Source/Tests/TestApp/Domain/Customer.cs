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

        public Customer(
            string name,
            List<LtcPharmacy> ltcPharmacies, 
            List<int> someInts, 
            long someNumber
        )
        {
            Name = name;
            _ltcPharmacies = ltcPharmacies;
            _someInts = someInts;
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

        public SimpleAddress? Address { get; protected set; }
        public SimplePersonName? ContactName { get; protected set; }
        public PhoneNumber? ContactPhone { get; protected set; }

        public decimal SomeMoney { get; protected set; }
        public long SomeNumber { get; protected set; }

        //
        // RELATIONSHIPS
        //

        private readonly List<LtcPharmacy> _ltcPharmacies = new List<LtcPharmacy>();
        public IReadOnlyCollection<LtcPharmacy> LtcPharmacies => _ltcPharmacies;

        private readonly List<int> _someInts = new List<int>();
        public IReadOnlyCollection<int> SomeInts => _someInts;

        //
        // OPERATIONS
        //

        public void SetName(string newName)
        {
            Name = newName;
        }

        public void SetAddress(SimpleAddress? address)
        {
            Address = address;
            Raise(new CustomerAddressChangedEvent(Id));
        }

        public void SetContact(SimplePersonName? contactName, PhoneNumber? contactPhone)
        {
            ContactName = contactName;
            ContactPhone = contactPhone;
        }

        public void AddLtcPharmacy(string name)
        {
            _ltcPharmacies.Add(new LtcPharmacy(name));
        }

        public void RemoveBar(LtcPharmacyId id)
        {
            _ltcPharmacies.RemoveAll(p => p.Id == id);
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