using System;
using Iti.Auth;
using Iti.Core.DateTime;
using Iti.Core.Services;
using Iti.Core.UnitOfWork;
using Iti.Passwords;
using Iti.ValueObjects;

namespace Iti.Authentication
{
    public class BaseAuthenticationService : ApplicationService, IAuthenticationService
    {
        private readonly AuthenticationServiceSettings _settings;
        private readonly IAuthContext _auth;
        private readonly IAuthenticationRepository _repo;
        private readonly IPasswordEncoder _pwEncoder;

        public BaseAuthenticationService(AuthenticationServiceSettings settings, IAuthContext auth, IAuthenticationRepository repo, IPasswordEncoder pwEncoder)
        {
            _settings = settings;
            _auth = auth;
            _repo = repo;
            _pwEncoder = pwEncoder;
        }

        public IAuthenticationId Login(string identifier, string password)
        {
            try
            {
                Authorize.Unauthenticated();

                using (var uow = UnitOfWork.Begin())
                {
                    var authUser = _repo.GetByLogin(identifier);
                    if (authUser == null)
                        throw new LoginFailedException("Invalid user or password");

                    if (!authUser.IsActive)
                        throw new LoginFailedException("User login is disabled");

                    if (!_pwEncoder.Validate(password, authUser.Password))
                        throw new LoginFailedException("Invalid user or password");

                    uow.Commit();

                    return authUser.Id;
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void ChangePassword(IAuthenticationId id, string currentPassword, string newPassword)
        {
            try
            {
                Authorize.AnyUser(_auth);

                using (var uow = UnitOfWork.Begin())
                {
                    var authUser = _repo.Get(id);
                    if (authUser == null)
                        return;

                    if (authUser.Password.HasValue())
                    {
                        if (!_pwEncoder.Validate(currentPassword, authUser.Password))
                            throw new IncorrectPasswordException();
                    }

                    if (!_pwEncoder.IsValid(newPassword))
                        throw new InvalidPasswordException();

                    var encpw = _pwEncoder.Encode(newPassword);
                    _repo.SetPassword(id, encpw);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public void RequestPasswordReset(EmailAddress email)
        {
            try
            {
                Authorize.Unauthenticated();

                using (var uow = UnitOfWork.Begin())
                {
                    var authUser = _repo.Get(email);
                    if (authUser == null)
                        return;

                    var pwrk = new PasswordResetKey(authUser, email);
                    _repo.Add(pwrk);

                    uow.Commit();
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public bool PasswordResetIsValid(string resetKey)
        {
            try
            {
                Authorize.Unauthenticated();

                using (var uow = UnitOfWork.Begin())
                {
                    var pwrk = _repo.GetPasswordResetKey(resetKey);
                    if (pwrk == null)
                        return false;

                    return pwrk.DateCreatedUtc >= DateTimeService.UtcNow.AddHours(-1 * _settings.PasswordResetKeyLifetimeHours);
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }

        public IAuthenticationId ResetPassword(string resetKey, string newPassword)
        {
            try
            {
                Authorize.Unauthenticated();

                using (var uow = UnitOfWork.Begin())
                {
                    var pwrk = _repo.GetPasswordResetKey(resetKey);
                    if (pwrk == null)
                        throw new InvalidResetKeyException();

                    if (!_pwEncoder.IsValid(newPassword))
                        throw new InvalidPasswordException();

                    var encpw = _pwEncoder.Encode(newPassword);
                    var authUserId = _repo.AuthenticationIdFromString(pwrk.AuthUserId);
                    _repo.SetPassword(authUserId, encpw);

                    _repo.Remove(pwrk.Id);

                    uow.Commit();

                    return authUserId;
                }
            }
            catch (Exception exc)
            {
                Handle(exc);
                throw;
            }
        }
    }
}