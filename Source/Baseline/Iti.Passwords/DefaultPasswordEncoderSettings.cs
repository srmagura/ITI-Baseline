namespace Iti.Passwords
{
    public class DefaultPasswordEncoderSettings
    {
        public int SaltByteSize { get; set; } = 24;
        public int HashByteSize { get; set; } = 24;
        public int Pbkdf2Iterations { get; set; } = 1000;
    }
}