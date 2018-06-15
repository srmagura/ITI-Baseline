using Iti.Passwords;
using Iti.ValueObjects;

namespace Iti.AuthService
{
    public interface IAuthenticationRepository
    {
        IAuthenticationUser GetByLogin(string login);
        IAuthenticationUser Get(IAuthenticationId id);
        IAuthenticationUser Get(EmailAddress email);

        IAuthenticationId AuthenticationIdFromString(string idString);

        void SetPassword(IAuthenticationId id, EncodedPassword encpw);

        void Add(PasswordResetKey pwrk);
        PasswordResetKey GetPasswordResetKey(string resetKey);

        void Remove(PasswordResetKeyId pwrkId);
    }
}