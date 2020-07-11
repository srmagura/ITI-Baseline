using System;
using Iti.Baseline.Core.Entites;

namespace Domain
{
    public class FooId : Identity
    {
        public FooId() { }
        public FooId(Guid guid) : base(guid) { }
        public FooId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}