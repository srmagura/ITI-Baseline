using ITI.DDD.Domain;
using System;

namespace TestApp.Domain.Identities
{
    public record OnCallProviderId : Identity
    {
        public OnCallProviderId() { }
        public OnCallProviderId(Guid guid) : base(guid) { }
        public OnCallProviderId(Guid? guid) : base(guid ?? default) { }
    }
}
