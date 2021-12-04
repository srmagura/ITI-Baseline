using ITI.DDD.Domain;
using System;

namespace TestApp.Domain.Identities
{
    public record FacilityId : Identity
    {
        public FacilityId() { }
        public FacilityId(Guid guid) : base(guid) { }
        public FacilityId(Guid? guid) : base(guid ?? default) { }
    }
}
