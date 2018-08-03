using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Iti.Voice
{
    public class TwilioVoiceSender : IVoiceSender
    {
        private readonly ITwilioVoiceSettings _settings;

        public TwilioVoiceSender(ITwilioVoiceSettings settings)
        {
            _settings = settings;
        }

        public void Send(string toPhoneNumber, string content, string callbackUrl)
        {
            TwilioClient.Init(_settings.Sid, _settings.AuthToken);

            var toPhone = new PhoneNumber($"+1{toPhoneNumber}");
            var fromPhone = new PhoneNumber($"+1{_settings.FromPhone}");

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