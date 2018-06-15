using System.Collections.Generic;

namespace Iti.Core.UserTracker
{
    public interface IUserTracker
    {
        int ActiveUserCount(int activeInLastMinutes);
        List<UserTrackDto> ActiveUsers(int activeInLastMinutes, int skip = 0, int take = 100);

        void OnUserAppServiceAccess(string userId, string service);
    }
}