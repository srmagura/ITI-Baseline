namespace Iti.Email
{
    public class EmailJobSettings
    {
        public int EmailMaxRetries { get; set; } = 24;
        public double EmailRetryMinutes { get; set; } = 5;
        public int EmailLifetimeDays { get; set; } = 90;
    }
}