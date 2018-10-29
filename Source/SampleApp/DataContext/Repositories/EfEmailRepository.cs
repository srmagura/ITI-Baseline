using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Core.DateTime;
using Iti.Core.Repositories;
using Iti.Email;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories
{
    public class EfEmailRepository : Repository<SampleDataContext>, IEmailRepository
    {
        public void Add(EmailRecord rec)
        {
            Context.EmailRecords.Add(rec);
        }

        public EmailRecord Get(long id)
        {
            return Context.EmailRecords.FirstOrDefault(p => p.Id == id);
        }

        public void ForEachPendingOrRetry(Action<EmailRecord> callback)
        {
            using (var db = new SampleDataContext())
            {
                var utcNow = DateTimeService.UtcNow;

                var pending = db.EmailRecords
                    .Where(p => p.Status == EmailStatus.Pending
                                && (p.NextRetryUtc == null || p.NextRetryUtc <= utcNow))
                    .OrderBy(p => p.DateCreatedUtc)
                    .ToList();

                foreach (var item in pending)
                {
                    callback(item);
                    db.SaveChanges();
                }
            }
        }

        public void CleanupOldEmails(int olderThanDays)
        {
            using (var db = new SampleDataContext())
            {
                var dt = DateTimeService.UtcNow.AddDays(-1 * olderThanDays);

                db.Database.ExecuteSqlCommand("DELETE FROM EmailRecords WHERE DateCreatedUtc < {0}", dt);
                db.SaveChanges();   // not technically necessary... *shrug*
            }
        }
    }
}