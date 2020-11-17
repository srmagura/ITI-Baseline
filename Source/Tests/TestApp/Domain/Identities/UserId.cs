using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain.Identities
{
    public class UserId : Identity
    {
        public UserId() { }
        public UserId(Guid guid) : base(guid) { }
        public UserId(Guid? guid) : base(guid ?? default) { }
    }
}
