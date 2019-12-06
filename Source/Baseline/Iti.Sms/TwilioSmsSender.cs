using Iti.Identities;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Iti.Sms
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly ITwilioSmsSettings _settings;

        public TwilioSmsSender(ITwilioSmsSettings settings)
        {
            _settings = settings;
        }

        public static PhoneNumber FormatTwilioPhone(string phone)
        {
            if (phone.StartsWith("+1"))
                return new PhoneNumber(phone);

            if(phone.StartsWith("1"))
                return new PhoneNumber($"+{phone}");

            return new PhoneNumber($"+1{phone}");
        }

        public void Send(NotificationId notificationId, string toAddress, string body)
        {
            TwilioClient.Init(_settings.Sid, _settings.AuthToken);
            var message = MessageResource.Create(
                to: FormatTwilioPhone(toAddress), // new PhoneNumber($"+1{toAddress}"),
                from: FormatTwilioPhone(_settings.FromPhone), // new PhoneNumber($"+1{_settings.FromPhone}"),
                body: body);
        }
    }
}