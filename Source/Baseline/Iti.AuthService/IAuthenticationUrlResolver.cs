namespace Iti.AuthService
{
    public interface IAuthenticationUrlResolver
    {
        string PasswordResetUrl(string resetKey);
    }
}