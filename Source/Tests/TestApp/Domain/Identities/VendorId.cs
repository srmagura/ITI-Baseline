﻿using ITI.DDD.Domain.Entities;
using System;

namespace TestApp.Domain.Identities
{
    public record VendorId : Identity
    {
        public VendorId() { }
        public VendorId(Guid guid) : base(guid) { }
        public VendorId(Guid? guid) : base(guid ?? default) { }
    }
}
