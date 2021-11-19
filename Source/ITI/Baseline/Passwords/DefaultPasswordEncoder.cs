using ITI.DDD.Core;
using ITI.DDD.Core.Util;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace ITI.Baseline.Passwords
{
    public class DefaultPasswordEncoder : IPasswordEncoder
    {
        private readonly DefaultPasswordEncoderSettings _settings;

        public DefaultPasswordEncoder(DefaultPasswordEncoderSettings settings)
        {
            _settings = settings;
        }

        public EncodedPassword Encode(string password)
        {
            // Generate a random salt
            using var rng = RandomNumberGenerator.Create(); 
            var salt = new byte[_settings.SaltByteSize];
            rng.GetBytes(salt);

            // Hash the password and encode the parameters
            var hash = PBKDF2(password, salt, _settings.Pbkdf2Iterations, _settings.HashByteSize);

            var enc = $"{_settings.Pbkdf2Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";

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