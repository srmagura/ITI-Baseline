using System;
using Iti.Core.Entites;

namespace Iti.Identities
{
    public class SmsRecordId : Identity
    {
        public SmsRecordId() { }
        public SmsRecordId(Guid guid) : base(guid) { }
        public SmsRecordId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}