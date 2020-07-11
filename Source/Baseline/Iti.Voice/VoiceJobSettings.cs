namespace Iti.Baseline.Voice
{
    public class VoiceJobSettings
    {
        public int VoiceMaxRetries { get; set; } = 24;
        public double VoiceRetryMinutes { get; set; } = 5;
        public int VoiceLifetimeDays { get; set; } = 90;
    }
}