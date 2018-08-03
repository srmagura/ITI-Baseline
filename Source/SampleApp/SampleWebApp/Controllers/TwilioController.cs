using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Twilio.Types;

namespace SampleWebApp.Controllers
{
    public class TwilioController : Controller
    {
        public IActionResult SendMessage(string toPhone, string name)
        {
            var model = "???";

            if (toPhone == null)
                toPhone = "9193243537";

            if (name != null)
            {
                name = name.ToLower();
                if (name == "john")
                    toPhone = "9192801222";
                else if (name == "work")
                    toPhone = "9193243537";
                else if (name == "sam")
                    toPhone = "9194122710";
                else if (name == "kelly")
                    toPhone = "9196082490";
            }

            try
            {
                // (984) 205-6622
                const string accountSid = "AC0d18c2921ce987e339accac9ab9e027d";
                const string authToken = "509a11a2592d41c6b553fd5cc90d1e07";
                TwilioClient.Init(accountSid, authToken);

                var to = new PhoneNumber($"+1{toPhone}");
                var from = new PhoneNumber("+19842056622");
                var call = CallResource.Create(to, from,

                    url: new Uri("http://itibaseline.azurewebsites.net/Twilio/GetIntro")

                    // url: new Uri("http://itibaseline.azurewebsites.net/Twilio/GetMessage"),
                    // machineDetection: "DetectMessageEnd",
                    // machineDetectionTimeout: 5

                    );

                model = $"{toPhone}: {call.Status}";
            }
            catch (Exception exc)
            {
                model = exc.ToString();
            }

            return PartialView((object)model);
        }

        public IActionResult GetIntro()
        {
            var response = new VoiceResponse();

            response.Pause(1);

            var gather = new Gather(
                new List<Gather.InputEnum>() { Gather.InputEnum.Dtmf, Gather.InputEnum.Speech },
                timeout: 10, 
                numDigits: 1,
                action: new Uri("http://itibaseline.azurewebsites.net/Twilio/GetMessage")
                );
            for (var i = 0; i < 3; i++)
            {
                gather.Say("This is an important message from Care Services.", voice: Twilio.TwiML.Voice.Say.VoiceEnum.Alice);
                gather.Pause(1);
                gather.Say("Please press 1 or say continue to retrieve your message.", voice: Twilio.TwiML.Voice.Say.VoiceEnum.Alice);
                gather.Pause(3);
            }

            response.Append(gather);

            Say(response, "We did not understand your response.  Please wait while we retrieve your message.");
            // BuildMessage(response);
            //response.Pause(1);
            response.Redirect(new Uri("http://itibaseline.azurewebsites.net/Twilio/GetMessage"));

            return Content(response.ToString(), "text/xml");
        }

        public IActionResult GetMessage()
        {
            var response = new VoiceResponse();

            response.Pause(length: 2);
            BuildMessage(response);

            return Content(response.ToString(), "text/xml");
        }

        private void BuildMessage(VoiceResponse response)
        {
            Say(response, "Hello");
            // Say(response, "This is an message from Care Services.");
            Say(response, "Adjudication Order 1 2 3 4 5 for patients Sam Magura and Jack Swanson has been completed.");
            Say(response, "All hail our new robot over lords!");
            Say(response, "The uprising has begun.");
            Say(response, "Your life force termination has been scheduled.");
            Say(response, "Goodbye puny human.");
            response.Pause(length: 2);
        }

        private void Say(VoiceResponse response, string text, int? loop = null)
        {
            response.Say(text, Twilio.TwiML.Voice.Say.VoiceEnum.Alice, loop, Twilio.TwiML.Voice.Say.LanguageEnum.EnUs);
            response.Pause(length: 1);
        }
    }
}