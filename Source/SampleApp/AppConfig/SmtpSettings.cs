using Iti.Baseline.Email;

namespace AppConfig
{
    public class SmtpSettings : ISmtpSettings
    {
        public string FromEmailAddress { get; set; } = "atlas@iticentral.com";
        public string FromDisplayName { get; set; } = "Support - Interface Technologies, Inc.";
        public string SmtpServer { get; set; } = "smtp.office365.com";
        public int SmtpPort { get; set; } = 587;
        public string SmtpUser { get; set; } = "atlas@iticentral.com";
        public string SmtpPassword { get; set; } = "TVWcT5anbFV8g3UL";
        public bool Secure { get; set; } = true;
    }
}
