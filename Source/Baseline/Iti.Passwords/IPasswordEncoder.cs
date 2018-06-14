namespace Iti.Passwords
{
    public interface IPasswordEncoder
    {
        string Encode(string plainTextPassword);
        bool Validate(string password, string userEncPassword);
        bool IsValid(string newPassword);
    }
}