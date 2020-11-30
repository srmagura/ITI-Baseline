using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Identities;

namespace TestApp.Domain
{
    public class LtcPharmacy : Member<Customer>
    {
        [Obsolete("AutoMapper only")]
        protected LtcPharmacy() { }

        public LtcPharmacy(string name)
        {
            SetName(name);
        }

        //
        // IDENTITY
        //

        public LtcPharmacyId Id { get; protected set; } = new LtcPharmacyId();

        //
        // ATTRIBUTES
        //

        public string? Name { get; protected set; }

        //
        // OPERATIONS
        //

        internal void SetName(string name)
        {
            Name = name ?? throw new ValidationException("Name");
        }
    }
}
