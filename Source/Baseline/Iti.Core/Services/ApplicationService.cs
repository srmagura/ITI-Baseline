using System;
using Iti.Auth;
using Iti.Core.UserTracker;
using Iti.Exceptions;
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

        protected T Command<T>(Action authorize, Func<T> exec)
        {
            try
            {
                using (var uow = UnitOfWork.UnitOfWork.Begin())
                {
                    authorize();

                    var result = exec();

                    uow.Commit();

                    return result;
                }
            }
            catch (EntityNotFoundException enfExc)
            {
                Handle(enfExc);
                return default(T);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        protected void Command(Action authorize, Action exec)
        {
            try
            {
                using (var uow = UnitOfWork.UnitOfWork.Begin())
                {
                    authorize();

                    exec();

                    uow.Commit();
                }
            }
            catch (EntityNotFoundException enfExc)
            {
                Handle(enfExc);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        protected T Query<T>(Action authorize, Func<T> exec)
        {
            try
            {
                using (UnitOfWork.UnitOfWork.Begin())
                {
                    authorize();

                    return exec();
                }
            }
            catch (EntityNotFoundException enfExc)
            {
                Handle(enfExc);
                return default(T);
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
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
            if (exc is DomainException dexc)
            {
                if (!dexc.AppServiceShouldLog)
                    return;
            }

            // by default, we log it... if we ever need more (like admin alerts) we'll address then
            Log.Error("Unhandled application exception", exc);
        }
    }
}