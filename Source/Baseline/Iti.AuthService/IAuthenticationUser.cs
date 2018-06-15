using Iti.Passwords;

namespace Iti.AuthService
{
    public interface IAuthenticationUser
    {
        IAuthenticationId Id { get; }
        string IdAsString();

        bool IsActive { get; }
        EncodedPassword Password { get; }
    }
}