using AppConfig;
using Iti.Baseline.Sms;
using Iti.Baseline.Voice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class TwilioPhoneNumberTests
    {
        [TestMethod]
        public void TestTwilioPhones()
        {
            DoTest("9198675309");
            DoTest("19198675309");
            DoTest("+19198675309");
        }

        private void DoTest(string ph)
        {
            var expected = "+19198675309";

            var phSms = TwilioSmsSender.FormatTwilioPhone(ph);
            Assert.AreEqual(expected, phSms.ToString());

            var phVoice = TwilioVoiceSender.FormatTwilioPhone(ph);
            Assert.AreEqual(expected, phVoice.ToString());
        }
    }
}