using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Iti.Sms
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly ITwilioSmsSettings _settings;

        public TwilioSmsSender(ITwilioSmsSettings settings)
        {
            _settings = settings;
        }

        public void Send(string toAddress, string body)
        {
            TwilioClient.Init(_settings.Sid, _settings.AuthToken);
            var message = MessageResource.Create(
                to: new Twilio.Types.PhoneNumber($"+1{toAddress}"),
                from: new Twilio.Types.PhoneNumber($"+1{_settings.FromPhone}"),
                body: body);
        }
    }
}