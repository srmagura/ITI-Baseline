
using System;
using Iti.Core.Entites;

namespace Domain
{
    public class BarId : Identity
    {
        public BarId() { }
        public BarId(Guid guid) : base(guid) { }
        public BarId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}