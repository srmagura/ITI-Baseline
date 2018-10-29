using System;
using System.Linq;
using Iti.Core.DateTime;
using Iti.Core.Repositories;
using Iti.Voice;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories
{
    public class EfVoiceRepository : Repository<SampleDataContext>, IVoiceRepository
    {
        public void Add(VoiceRecord rec)
        {
            Context.VoiceRecords.Add(rec);
        }

        public VoiceRecord Get(long id)
        {
            return Context.VoiceRecords.FirstOrDefault(p => p.Id == id);
        }

        public void ForEachPendingOrRetry(Action<VoiceRecord> callback)
        {
            using (var db = new SampleDataContext())
            {
                var now = DateTimeService.UtcNow;

                var pending = db.VoiceRecords
                    .Where(p => p.Status == VoiceStatus.Pending
                                && (p.NextRetryUtc == null || p.NextRetryUtc <= now))
                    .ToList();

                foreach (var item in pending)
                {
                    callback(item);
                }
            }
        }

        public void CleanupOldVoice(int olderThanDays)
        {
            using (var db = new SampleDataContext())
            {
                var dt = DateTimeService.UtcNow.AddDays(-1 * olderThanDays);

                db.Database.ExecuteSqlCommand("DELETE FROM VoiceRecords WHERE DateCreatedUtc < {0}", dt);
                db.SaveChanges();   // not technically necessary... *shrug*
            }
        }
    }
}