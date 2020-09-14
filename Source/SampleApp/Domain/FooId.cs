using System;
using Iti.Baseline.Core.Entities;

namespace Domain
{
    public class FooId : Identity
    {
        public FooId() { }
        public FooId(Guid guid) : base(guid) { }
        public FooId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}