using System;
using System.Collections.Generic;
using Iti.Core.DateTime;
using Iti.Core.UserTracker;

namespace Iti.Core.Services
{
    public class ConsoleUserTracker : IUserTracker
    {
        public int ActiveUserCount(int activeInLastMinutes)
        {
            return 0;
        }

        public List<UserTrackDto> ActiveUsers(int activeInLastMinutes, int skip = 0, int take = 100)
        {
            return new List<UserTrackDto>();
        }

        public void OnUserAppServiceAccess(string userId, string service)
        {
            Console.WriteLine($"USER TRACKER: {DateTimeService.UtcNow}: {userId}: {service}");
        }
    }
}