using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Sms;

namespace CoreTests.Helpers
{
    public class TestSmsRepository : ISmsRepository
    {
        public static List<SmsRecord> Records;
        public static bool WasCleanedUp = false;

        public static void Initialize(List<SmsRecord> emailRecords)
        {
            Records = emailRecords;
            WasCleanedUp = false;
        }

        //

        public void Add(SmsRecord rec)
        {
            throw new TestNotImplementedException();
        }

        public SmsRecord Get(long id)
        {
            throw new TestNotImplementedException();
        }

        public void ForEachPendingOrRetry(Action<SmsRecord> callback)
        {
            var pending = Records
                .Where(p => p.Status == SmsStatus.Pending)
                .ToList();

            foreach (var rec in pending)
            {
                callback(rec);
            }
        }

        public void CleanupOldSms(int olderThanDays)
        {
            WasCleanedUp = true;
        }
    }
}