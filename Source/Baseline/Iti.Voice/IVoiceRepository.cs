using Iti.Baseline.Identities;

namespace Iti.Baseline.Voice
{
    public interface IVoiceRepository
    {
        void Add(VoiceRecord rec);
        VoiceRecord Get(VoiceRecordId id);
    }
}