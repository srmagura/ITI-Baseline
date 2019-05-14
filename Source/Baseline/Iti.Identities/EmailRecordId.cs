using System;
using Iti.Core.Entites;

namespace Iti.Identities
{
    public class EmailRecordId : Identity
    {
        public EmailRecordId() { }
        public EmailRecordId(Guid guid) : base(guid) { }
        public EmailRecordId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}