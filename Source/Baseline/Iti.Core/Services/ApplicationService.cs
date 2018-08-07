using System;
using Iti.Auth;
using Iti.Core.UserTracker;
using Iti.Inversion;
using Iti.Logging;
using Iti.Utilities;

namespace Iti.Core.Services
{
    public abstract class ApplicationService
    {
        protected readonly IAuthContext Authorize;

        protected ApplicationService(IAuthContext baseAuth)
        {
            Authorize = baseAuth;

            if (this is IUserTracking ut)
            {
                TrackUser();
            }
        }

        private void TrackUser()
        {
            if (Authorize == null)
                return;

            if (!Authorize.IsAuthenticated)
                return;

            if (Authorize.UserId.EqualsIgnoreCase("SYSTEM") || Authorize.UserName.EqualsIgnoreCase("SYSTEM"))
                return;

            var tracker = IOC.TryResolve<IUserTracker>();
            if (tracker == null)
                return;

            var userId = Authorize.UserId;
            var service = this.GetType().Name;
            tracker.OnUserAppServiceAccess(userId, service);
        }

        protected void Handle(Exception exc)
        {
            // by default, we log it... if we ever need more (like admin alerts) we'll address then
            Log.Error("Unhandled application exception", exc);
        }
    }
}