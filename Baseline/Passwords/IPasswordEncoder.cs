namespace ITI.Baseline.Passwords
{
    public interface IPasswordEncoder
    {
        EncodedPassword Encode(string password);
        bool IsCorrect(string password, EncodedPassword encodedPassword);
    }
}
