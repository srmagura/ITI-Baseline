using Iti.Passwords;

namespace Iti.Authentication
{
    public interface IAuthenticationUser
    {
        IAuthenticationId Id { get; }
        string IdAsString();

        bool IsActive { get; }
        EncodedPassword Password { get; }
    }
}