using System;
using Iti.Identities;

namespace Iti.Voice
{
    public interface IVoiceRepository
    {
        void Add(VoiceRecord rec);
        VoiceRecord Get(VoiceRecordId id);
    }
}