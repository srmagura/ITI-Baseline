using ITI.DDD.Domain.Entities;
using System;

namespace TestApp.Domain.Identities
{
    public record CustomerId : Identity
    {
        public CustomerId() { }
        public CustomerId(Guid guid) : base(guid) { }
        public CustomerId(Guid? guid) : base(guid ?? default) { }
    }
}
