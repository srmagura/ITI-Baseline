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
        private readonly IAuthContext _baseAuth;

        protected ApplicationService(IAuthContext baseAuth)
        {
            _baseAuth = baseAuth;

            if (this is IUserTracking ut)
            {
                TrackUser();
            }
        }

        private void TrackUser()
        {
            if (_baseAuth == null)
                return;

            if (!_baseAuth.IsAuthenticated)
                return;

            if (_baseAuth.UserId.EqualsIgnoreCase("SYSTEM") || _baseAuth.UserName.EqualsIgnoreCase("SYSTEM"))
                return;

            var tracker = IOC.TryResolve<IUserTracker>();
            if (tracker == null)
                return;

            var userId = _baseAuth.UserId;
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