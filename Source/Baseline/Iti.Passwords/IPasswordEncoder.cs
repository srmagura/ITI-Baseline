namespace Iti.Baseline.Passwords
{
    public interface IPasswordEncoder<TEncodedPassword>
    {
        TEncodedPassword Encode(string plainTextPassword);
        bool Validate(string password, TEncodedPassword userEncPassword);
        bool IsValid(string password);
    }
}