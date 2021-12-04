namespace ITI.Baseline.Passwords
{
    public interface IPasswordEncoder
    {
        EncodedPassword Encode(string plainTextPassword);
        bool IsCorrect(string password, EncodedPassword encodedPassword);
    }
}