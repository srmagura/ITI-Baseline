using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain.ValueObjects
{
    public class FacilityContact : ValueObject
    {
        public SimplePersonName? Name { get; set; }
        public EmailAddress? Email { get; set; }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Name;
            yield return Email;
        }
    }
}
