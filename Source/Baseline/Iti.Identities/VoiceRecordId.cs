using System;
using Iti.Baseline.Core.Entities;

namespace Iti.Baseline.Identities
{
    public class VoiceRecordId : Identity
    {
        public VoiceRecordId() { }
        public VoiceRecordId(Guid guid) : base(guid) { }
        public VoiceRecordId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}