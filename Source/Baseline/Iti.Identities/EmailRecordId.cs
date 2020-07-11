using System;
using Iti.Baseline.Core.Entites;

namespace Iti.Baseline.Identities
{
    public class EmailRecordId : Identity
    {
        public EmailRecordId() { }
        public EmailRecordId(Guid guid) : base(guid) { }
        public EmailRecordId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}