using System;
using Iti.Auth;
using Iti.Exceptions;
using Iti.Inversion;
using Iti.Logging;
using Iti.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.Services
{
    public abstract class ApplicationService
    {
        protected readonly IAuthContext Authorize;

        protected ApplicationService(IAuthContext baseAuth)
        {
            Authorize = baseAuth;
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

        protected void Handle(Exception exc)
        {
            if (exc is DbUpdateException)
            {
                HandleDbUpdateException(exc);
                return;
            }

            if (exc is DomainException dexc)
            {
                if (!dexc.AppServiceShouldLog)
                    return;
            }

            // by default, we log it... if we ever need more (like admin alerts) we'll address then
            Log.Error("Unhandled application exception", exc);
        }

        private void HandleDbUpdateException(Exception exc, int depth = 0)
        {
            if (exc == null)
                return;

            if (depth > 10)
                return;

            if (exc.Message.StartsWith("Cannot insert duplicate key"))
            {
                GetDbUpdateInfo(exc.Message, out var table, out var value);
                throw new DuplicateKeyException(table, value, exc);
            }
            else if (exc.InnerException != null)
            {
                HandleDbUpdateException(exc.InnerException, depth + 1);
            }
        }

        public static void GetDbUpdateInfo(string message, out string table, out string value)
        {
            table = "";
            value = "";

            if (message == null)
                return;

            // find TABLE

            var cols = message.Split('\'');
            if (cols.Length < 2)
                return;

            table = cols[1]?.Replace("dbo.", "") ?? "";

            // find VALUE
            var pos = message.IndexOf('(');
            if (pos >= 0)
            {
                value = message.Substring(pos + 1);
                pos = value.LastIndexOf(')');
                value = value.Substring(0, pos);
            }
        }
    }
}