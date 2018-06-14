using System;
using System.Collections.Generic;

namespace Iti.Sms
{
    public interface ISmsRepository
    {
        void Add(SmsRecord rec);
        SmsRecord Get(long id);

        void ForEachPendingOrRetry(Action<SmsRecord> callback);
        void CleanupOldSms(int olderThanDays);
    }
}