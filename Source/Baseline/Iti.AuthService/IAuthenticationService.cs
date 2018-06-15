using Iti.ValueObjects;

namespace Iti.AuthService
{
    public interface IAuthenticationService
    {
        IAuthenticationId Login(string identifier, string password);

        void ChangePassword(IAuthenticationId id, string currentPassword, string newPassword);

        void RequestPasswordReset(EmailAddress email);
        bool PasswordResetIsValid(string resetKey);
        IAuthenticationId ResetPassword(string resetKey, string newPassword);
    }
}