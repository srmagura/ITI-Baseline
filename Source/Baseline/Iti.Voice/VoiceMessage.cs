using Twilio.TwiML;

namespace Iti.Voice
{
    public class VoiceMessage : IVoiceMessage
    {
        private readonly VoiceResponse _response = new VoiceResponse();

        public Twilio.TwiML.Voice.Say.VoiceEnum Voice = Twilio.TwiML.Voice.Say.VoiceEnum.Alice;
        public Twilio.TwiML.Voice.Say.LanguageEnum Language = Twilio.TwiML.Voice.Say.LanguageEnum.EnUs;

        public string ContentMarkup => _response.ToString();

        public void Say(string message, int? loop = null)
        {
            if (string.IsNullOrEmpty(message))
                return;
            _response.Say(message, Voice, loop, Language);
        }

        public void Pause(int seconds)
        {
            if (seconds <= 0)
                seconds = 1;
            _response.Pause(seconds);
        }

        public override string ToString()
        {
            return _response.ToString();
        }

    }
}