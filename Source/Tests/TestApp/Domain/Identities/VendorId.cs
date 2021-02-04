using ITI.DDD.Domain.Entities;
using System;

namespace TestApp.Domain.Identities
{
    public class VendorId : Identity
    {
        public VendorId() { }
        public VendorId(Guid guid) : base(guid) { }
        public VendorId(Guid? guid) : base(guid ?? default) { }
    }
}
