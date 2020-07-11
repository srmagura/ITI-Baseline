using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Baseline.Identities;
using Iti.Baseline.Sms;

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

        public SmsRecord Get(SmsRecordId id)
        {
            throw new TestNotImplementedException();
        }
    }
}