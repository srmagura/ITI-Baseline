using ITI.DDD.Core;

namespace ITI.Baseline.Passwords;

public class DefaultPasswordValidator : IPasswordValidator
{
    public bool IsValid(string password)
    {
        if (password != password.Trim())
            return false;

        var hasUpperCase = false;
        var hasNonAlpha = false;

        if (!password.HasValue())
            return false;

        if (password.Length < 8)
            return false;

        foreach (var ch in password)
        {
            if (char.IsUpper(ch))
                hasUpperCase = true;
            else if (!char.IsLetter(ch))
                hasNonAlpha = true;
        }

        return hasUpperCase && hasNonAlpha;
    }
}
