namespace Iti.Sms
{
    public class SmsJobSettings
    {
        public int SmsMaxRetries { get; set; } = 24;
        public double SmsRetryMinutes { get; set; } = 5;
        public int SmsLifetimeDays { get; set; } = 90;
    }
}