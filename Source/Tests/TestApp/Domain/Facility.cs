using ITI.Baseline.Audit;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Domain
{
    // Test of nested value objects
    public class Facility : AggregateRoot
    {
        [Obsolete("Persistence use only")]
        protected Facility() { }

        public FacilityId Id { get; set; } = new FacilityId();
        public string? Name { get; set; }
        public FacilityContact? Contact { get; set; }

        public Facility(string name, FacilityContact? contact)
        {
            Name = name;
            SetContact(contact);
        }

        public void SetContact(FacilityContact? contact)
        {
            Contact = contact;
        }
    }
}
