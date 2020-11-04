using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Domain
{
    public class CustomerId : Identity
    {
        public CustomerId() { }
        public CustomerId(Guid guid) : base(guid) { }
        public CustomerId(Guid? guid) : base(guid ?? default) { }
    }
}
