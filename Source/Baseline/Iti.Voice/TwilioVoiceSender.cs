using System;
using Iti.Baseline.Identities;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Iti.Baseline.Voice
{
    public class TwilioVoiceSender : IVoiceSender
    {
        private readonly ITwilioVoiceSettings _settings;

        public TwilioVoiceSender(ITwilioVoiceSettings settings)
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

        public void Send(NotificationId notificationId, string toPhoneNumber, string callbackUrl)
        {
            TwilioClient.Init(_settings.Sid, _settings.AuthToken);

            var toPhone = FormatTwilioPhone(toPhoneNumber); //  new PhoneNumber($"+1{toPhoneNumber}");
            var fromPhone = FormatTwilioPhone(_settings.FromPhone); // new PhoneNumber($"+1{_settings.FromPhone}");

            if (_settings.UseMachineDetection)
            {
                var call = CallResource.Create(toPhone, fromPhone,
                    url: new Uri(callbackUrl),
                    machineDetection: "DetectMessageEnd",
                    machineDetectionTimeout: _settings.MachineDetectionTimeout
                );
            }
            else
            {
                var call = CallResource.Create(toPhone, fromPhone,
                    url: new Uri(callbackUrl)
                );
            }
        }
    }
}