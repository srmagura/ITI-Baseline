namespace Iti.Authentication
{
    public interface IAuthenticationUrlResolver
    {
        string PasswordResetUrl(string resetKey);
    }
}