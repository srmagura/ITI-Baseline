using ITI.DDD.Domain.Entities;
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
