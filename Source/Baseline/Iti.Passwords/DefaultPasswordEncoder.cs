using System;
using System.Linq;
using System.Security.Cryptography;
using Iti.Exceptions;
using Iti.Utilities;

namespace Iti.Passwords
{
    public class DefaultPasswordEncoder : IPasswordEncoder<EncodedPassword>
    {
        private readonly DefaultPasswordEncoderSettings _settings;

        // The following constants may be changed without breaking existing hashes.
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
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[_settings.SaltByteSize];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            var hash = PBKDF2(password, salt, _settings.Pbkdf2Iterations, _settings.HashByteSize);

            var enc = $"{_settings.Pbkdf2Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";

            return new EncodedPassword(enc);
        }

        public bool Validate(string password, EncodedPassword encodedPassword)
        {
            try
            {
                var enc = encodedPassword?.Value;
                if (enc == null)
                    throw new DomainException("Invalid Encoded Password type", true);

                // Extract the parameters from the hash
                char[] delimiter = { ':' };

                var split = enc.Split(delimiter);

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

        private bool BinaryEquals(byte[] hash, byte[] testHash)
        {
            return (hash.Length == testHash.Length) && !hash.Where((t, i) => t != testHash[i]).Any();
        }

        private byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations
            };

            return pbkdf2.GetBytes(outputBytes);
        }
    }
}