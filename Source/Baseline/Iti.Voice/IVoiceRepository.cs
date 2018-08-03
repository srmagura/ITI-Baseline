using System;

namespace Iti.Voice
{
    public interface IVoiceRepository
    {
        void Add(VoiceRecord rec);
        VoiceRecord Get(long id);

        void ForEachPendingOrRetry(Action<VoiceRecord> callback);
        void CleanupOldVoice(int olderThanDays);
    }
}