namespace ITI.Baseline.Passwords;

public interface IPasswordValidator
{
    bool IsValid(string password);
}
