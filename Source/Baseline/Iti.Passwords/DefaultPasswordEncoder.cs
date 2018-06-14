using System;
using System.Linq;
using System.Security.Cryptography;
using Iti.Utilities;

namespace Iti.Passwords
{
    public class DefaultPasswordEncoder : IPasswordEncoder
    {
        // The following constants may be changed without breaking existing hashes.
        public const int SaltByteSize = 24;
        public const int HashByteSize = 24;
        public const int Pbkdf2Iterations = 1000;

        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        public string Encode(string password)
        {
            // Generate a random salt
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[SaltByteSize];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            var hash = PBKDF2(password, salt, Pbkdf2Iterations, HashByteSize);

            return $"{Pbkdf2Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        public bool Validate(string password, string encodedPassword)
        {
            try
            {
                // Extract the parameters from the hash
                char[] delimiter = { ':' };

                var split = encodedPassword.Split(delimiter);

                var iterations = Int32.Parse(split[IterationIndex]);
                var salt = Convert.FromBase64String(split[SaltIndex]);
                var hash = Convert.FromBase64String(split[Pbkdf2Index]);

                var testHash = PBKDF2(password, salt, iterations, hash.Length);

                return BinaryEquals(hash, testHash);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValid(string newPassword)
        {
            var hasUpperCase = false;
            var hasNonAlpha = false;

            if (!newPassword.HasValue())
                return false;

            if (newPassword.Length < 8)
                return false;

            foreach (var ch in newPassword)
            {
                if (char.IsUpper(ch))
                    hasUpperCase = true;
                else if (!char.IsLetter(ch))
                    hasNonAlpha = true;
            }
            
            return hasUpperCase && hasNonAlpha;
        }

        private static bool BinaryEquals(byte[] hash, byte[] testHash)
        {
            return (hash.Length == testHash.Length) && !hash.Where((t, i) => t != testHash[i]).Any();
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations
            };

            return pbkdf2.GetBytes(outputBytes);
        }
    }
}