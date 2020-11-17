using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain.Identities
{
    public class LtcPharmacyId : Identity
    {
        public LtcPharmacyId() { }
        public LtcPharmacyId(Guid guid) : base(guid) { }
        public LtcPharmacyId(Guid? guid) : base(guid ?? default) { }
    }
}
