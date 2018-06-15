namespace Iti.Passwords
{
    public interface IPasswordEncoder
    {
        EncodedPassword Encode(string plainTextPassword);
        bool Validate(string password, EncodedPassword userEncPassword);
        bool IsValid(string password);
    }
}