using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Baseline.Email;
using Iti.Baseline.Identities;

namespace CoreTests.Helpers
{
    public class TestEmailRepository : IEmailRepository
    {
        public static List<EmailRecord> Records;
        public static bool WasCleanedUp = false;

        public static void Initialize(List<EmailRecord> emailRecords)
        {
            Records = emailRecords;
            WasCleanedUp = false;
        }

        //

        public void Add(EmailRecord rec)
        {
            throw new TestNotImplementedException();
        }

        public EmailRecord Get(EmailRecordId id)
        {
            throw new TestNotImplementedException();
        }
    }

    public class TestNotImplementedException : NotImplementedException
    {
    }
}