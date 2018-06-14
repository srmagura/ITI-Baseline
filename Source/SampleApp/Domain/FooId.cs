using System;
using Iti.Core.Entites;

namespace Domain
{
    public class FooId : Identity
    {
        public FooId() { }
        public FooId(Guid guid) : base(guid) { }
    }
}