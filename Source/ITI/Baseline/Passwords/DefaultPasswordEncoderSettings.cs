namespace ITI.Baseline.Passwords
{
    public class DefaultPasswordEncoderSettings
    {
        // The following constants may be changed without breaking existing hashes.
        public int SaltByteSize { get; set; } = 24;
        public int HashByteSize { get; set; } = 24;
        public int Pbkdf2Iterations { get; set; } = 1000;
    }
}