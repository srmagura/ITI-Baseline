using System;
using Iti.Core.Entites;

namespace Iti.Identities
{
    public class VoiceRecordId : Identity
    {
        public VoiceRecordId() { }
        public VoiceRecordId(Guid guid) : base(guid) { }
        public VoiceRecordId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}