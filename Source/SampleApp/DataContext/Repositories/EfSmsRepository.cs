using System;
using System.Linq;
using Iti.Core.DateTime;
using Iti.Core.Repositories;
using Iti.Sms;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories
{
    public class EfSmsRepository : Repository<SampleDataContext>, ISmsRepository
    {
        public void Add(SmsRecord rec)
        {
            Context.SmsRecords.Add(rec);
        }

        public SmsRecord Get(long id)
        {
            return Context.SmsRecords.FirstOrDefault(p => p.Id == id);
        }

        public void ForEachPendingOrRetry(Action<SmsRecord> callback)
        {
            using (var db = new SampleDataContext())
            {
                var now = DateTimeService.UtcNow;

                var pending = db.SmsRecords
                    .Where(p => p.Status == SmsStatus.Pending
                                && (p.NextRetry == null || p.NextRetry <= now))
                    .ToList();

                foreach (var item in pending)
                {
                    callback(item);
                }
            }
        }

        public void CleanupOldSms(int olderThanDays)
        {
            using (var db = new SampleDataContext())
            {
                var dt = DateTimeService.UtcNow.AddDays(-1 * olderThanDays);

                db.Database.ExecuteSqlCommand("DELETE FROM SmsRecords WHERE DateCreatedUtc < {0}", dt);
                db.SaveChanges();   // not technically necessary... *shrug*
            }
        }
    }
}