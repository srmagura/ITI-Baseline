using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain
{
    public class LtcPharmacy : Member<Customer>
    {
        [Obsolete("AutoMapper only")]
        protected LtcPharmacy() { }

        public LtcPharmacy(string name)
        {
            Name = name;
        }

        //
        // IDENTITY
        //

        public LtcPharmacyId Id { get; protected set; } = new LtcPharmacyId();

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
