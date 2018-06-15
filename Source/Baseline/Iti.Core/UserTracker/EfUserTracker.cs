using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Core.DateTime;
using Iti.Inversion;
using Iti.Logging;
using Iti.Utilities;

namespace Iti.Core.UserTracker
{
    public class EfUserTracker : IUserTracker
    {
        public int ActiveUserCount(int activeInLastMinutes)
        {
            using (var db = IOC.TryResolve<IUserTrackingDataContext>())
            {
                if (db == null)
                {
                    Log.Warning("No IUserTrackingDataContext registered");
                    return 0;
                }

                var since = DateTimeService.UtcNow.AddMinutes(-1 * activeInLastMinutes);

                return db.UserTracks.Count(p => p.LastAccessUtc >= since);
            }
        }

        public List<UserTrackDto> ActiveUsers(int activeInLastMinutes, int skip, int take)
        {
            using (var db = IOC.TryResolve<IUserTrackingDataContext>())
            {
                if (db == null)
                {
                    Log.Warning("No IUserTrackingDataContext registered");
                    return null;
                }

                var since = DateTimeService.UtcNow.AddMinutes(-1 * activeInLastMinutes);

                var result = db.UserTracks
                    .Where(p => p.LastAccessUtc >= since)
                    .OrderByDescending(p => p.LastAccessUtc)
                    .Skip(skip)
                    .Take(take)
                    .Select(p => new UserTrackDto
                    {
                        LastAccessUtc = p.LastAccessUtc,
                        UserId = p.UserId,
                        Service = p.Service,
                    })
                    .ToList();

                return result;
            }
        }

        public void OnUserAppServiceAccess(string userId, string service)
        {
            try
            {
                using (var db = IOC.TryResolve<IUserTrackingDataContext>())
                {
                    if (db == null)
                    {
                        Log.Warning("No IUserTrackingDataContext registered");
                        return;
                    }

                    userId = userId.MaxLength(128);

                    var entry = db.UserTracks.FirstOrDefault(p => p.UserId == userId);
                    if (entry == null)
                    {

                        entry = new UserTrack(userId, service);
                        db.UserTracks.Add(entry);
                    }
                    else
                    {
                        entry.Update(service);
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Log.Error($"Could not log user track: {userId}|{service}", exc);
            }
        }
    }
}