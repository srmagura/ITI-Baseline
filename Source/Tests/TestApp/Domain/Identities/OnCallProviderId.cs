using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain.Identities
{
    public class OnCallProviderId : Identity
    {
        public OnCallProviderId() { }
        public OnCallProviderId(Guid guid) : base(guid) { }
        public OnCallProviderId(Guid? guid) : base(guid ?? default) { }
    }
}
