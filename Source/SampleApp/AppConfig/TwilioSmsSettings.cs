using Iti.Sms;

namespace AppConfig
{
    public class TwilioSmsSettings : ITwilioSmsSettings
    {
        public string Sid { get; set; } = "AC0d18c2921ce987e339accac9ab9e027d";
        public string AuthToken { get; set; } = "509a11a2592d41c6b553fd5cc90d1e07";
        public string FromPhone { get; set; } = "9842056622";
    }
}
