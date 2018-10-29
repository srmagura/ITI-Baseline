using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Email;

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

        public EmailRecord Get(long id)
        {
            throw new TestNotImplementedException();
        }

        public void ForEachPendingOrRetry(Action<EmailRecord> callback)
        {
            var pending = Records
                .Where(p => p.Status == EmailStatus.Pending)
                .ToList();

            foreach (var rec in pending)
            {
                callback(rec);
            }
        }

        public void CleanupOldEmails(int olderThanDays)
        {
            WasCleanedUp = true;
        }


    }

    public class TestNotImplementedException : NotImplementedException
    {
    }
}