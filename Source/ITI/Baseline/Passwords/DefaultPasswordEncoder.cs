using ITI.DDD.Core;
using ITI.DDD.Core.Util;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace ITI.Baseline.Passwords
{
    public class DefaultPasswordEncoder : IPasswordEncoder<EncodedPassword>
    {
        private readonly DefaultPasswordEncoderSettings _settings;

        // Changing these will break existing hashes
        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        public DefaultPasswordEncoder(DefaultPasswordEncoderSettings settings)
        {
            _settings = settings;
        }

        public EncodedPassword Encode(string password)
        {
            // Generate a random salt
            using var rng = new RNGCryptoServiceProvider();
            var salt = new byte[_settings.SaltByteSize];
            rng.GetBytes(salt);

            // Hash the password and encode the parameters
            var hash = PBKDF2(password, salt, _settings.Pbkdf2Iterations, _settings.HashByteSize);

            var enc = $"{_settings.Pbkdf2Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";

            return new EncodedPassword(enc);
        }

        public bool Validate(string password, EncodedPassword encodedPassword)
        {
            var enc = encodedPassword?.Value;
            if (enc == null)
                throw new DomainException("Invalid Encoded Password type.", DomainException.AppServiceLogAs.Error);

            // Extract the parameters from the hash
            char[] delimiter = { ':' };

            var split = enc.Split(delimiter);

            var iterations = int.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

            var testHash = PBKDF2(password, salt, iterations, hash.Length);

            return BinaryEquals(hash, testHash);
        }

        public bool IsValid(string password)
        {
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
}