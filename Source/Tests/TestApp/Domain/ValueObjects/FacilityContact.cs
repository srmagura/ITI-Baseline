using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain.ValueObjects
{
    public class FacilityContact : ValueObject
    {
        [Obsolete("Persistence use only")]
        protected FacilityContact() { }

        public FacilityContact(SimplePersonName? name, EmailAddress? email)
        {
            Name = name;
            Email = email;
        }

        public SimplePersonName? Name { get; set; }
        public EmailAddress? Email { get; set; }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Name;
            yield return Email;
        }
    }
}
