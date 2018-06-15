using System.Collections.Generic;

namespace Iti.Core.UserTracker
{
    public class NullUserTracker : IUserTracker
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
            // DO NOTHING
        }
    }
}