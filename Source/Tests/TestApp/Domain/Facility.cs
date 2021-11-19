using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.Entities;
using System;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.Domain
{
    // Test of nested value objects
    public class Facility : AggregateRoot
    {
        public Facility(string name, FacilityContact contact)
        {
            Name = name;

            Require.NotNull(contact, "Contact is required.");
            Contact = contact;
        }

        public FacilityId Id { get; protected set; } = new FacilityId();
        public string Name { get; protected set; }

        public FacilityContact Contact { get; protected set; } 

        public void SetContact(FacilityContact contact)
        {
            Require.NotNull(contact, "Contact is required.");
            Contact = contact;
        }
    }
}
