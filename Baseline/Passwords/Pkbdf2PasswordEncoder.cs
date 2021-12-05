using System.Security.Cryptography;
using ITI.DDD.Core;

namespace ITI.Baseline.Passwords;

public class Pkbdf2PasswordEncoder : IPasswordEncoder
{
    // The following constants may be changed without breaking existing hashes. BUT TEST IT FIRST TO SEE IF THIS IS ACTUALLY TRUE
    private const int SaltByteSize = 24;
    private const int HashByteSize = 24;
    private const int Pbkdf2Iterations = 1000;

    public EncodedPassword Encode(string password)
    {
        // Generate a random salt
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltByteSize];
        rng.GetBytes(salt);

        // Hash the password and encode the parameters
        var hash = PBKDF2(password, salt, Pbkdf2Iterations, HashByteSize);

        var enc = $"{Pbkdf2Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";

        return new EncodedPassword(enc);
    }

    public bool IsCorrect(string password, EncodedPassword encodedPassword)
    {
        var enc = encodedPassword?.Value;
        if (enc == null)
            throw new DomainException("Invalid Encoded Password type.", DomainException.AppServiceLogAs.Error);

        // Extract the parameters from the hash
        char[] delimiter = { ':' };

        var split = enc.Split(delimiter);

        var iterations = int.Parse(split[0]);
        var salt = Convert.FromBase64String(split[1]);
        var hash = Convert.FromBase64String(split[2]);

        var testHash = PBKDF2(password, salt, iterations, hash.Length);

        return BinaryEquals(hash, testHash);
    }

    private static bool BinaryEquals(byte[] hash, byte[] testHash)
    {
        return (hash.Length == testHash.Length) && !hash.Where((t, i) => t != testHash[i]).Any();
    }

    private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt)
        {
            IterationCount = iterations
        };

        return pbkdf2.GetBytes(outputBytes);
    }
}
