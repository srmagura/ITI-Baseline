using Iti.Baseline.Identities;

namespace Iti.Baseline.Sms
{
    public interface ISmsRepository
    {
        void Add(SmsRecord rec);
        SmsRecord Get(SmsRecordId id);
    }
}