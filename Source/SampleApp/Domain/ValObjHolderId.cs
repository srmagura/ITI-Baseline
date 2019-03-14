using System;
using Iti.Core.Entites;

namespace Domain
{
    public class ValObjHolderId : Identity
    {
        public ValObjHolderId() { }
        public ValObjHolderId(Guid guid) : base(guid) { }
        public ValObjHolderId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}