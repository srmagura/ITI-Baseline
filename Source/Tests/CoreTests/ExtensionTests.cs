using System;
using System.Linq;
using Iti.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeZone =Iti.Core.DateTime.TimeZone;

namespace BasicTests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void CleanPhone()
        {
            Assert.AreEqual(null, ((string)null).DigitsOnly());
            Assert.AreEqual("9192801222", "919.280.1222".DigitsOnly());
            Assert.AreEqual("9192801222", "(919) 280-1222".DigitsOnly());
            Assert.AreEqual("12345", ",1,2,3,4,5,,,".DigitsOnly());
        }

        [TestMethod]
        public void ValidPhone()
        {
            Assert.IsTrue("9192801222".IsValidPhone());
            Assert.IsTrue("91928012221234".IsValidPhone());

            Assert.IsFalse(((string)null).IsValidPhone());
            Assert.IsFalse("1234".IsValidPhone());
        }

        [TestMethod]
        public void TimeZoneIdLengths()
        {
            var allWindows = TimeZoneInfo.GetSystemTimeZones();

            var maxWindowsId = allWindows.Max(p => p.Id.Length);
            Console.WriteLine($"Max Windows Id: {maxWindowsId}");

            var maxWindowsName = allWindows.Max(p => p.DisplayName.Length);
            Console.WriteLine($"Max Windows Name: {maxWindowsName}");

            Assert.IsTrue(maxWindowsId < 64);
            Assert.IsTrue(maxWindowsName < 64);
        }

        [TestMethod]
        public void DumpTimeZones()
        {
            var all = TimeZoneInfo.GetSystemTimeZones();

            // var tzs = all.Where(p => p.DisplayName.Contains("US")).ToList();
            var tzs = all.OrderBy(p => p.Id).ToList();

            foreach (var tz in tzs)
            {
                Console.WriteLine($"{tz.Id}: {tz.DisplayName}");

                // Assert.AreEqual(tz.Id, back);
            }
        }

        [TestMethod]
        public void TimeZoneUtcConvert()
        {
            var utcnow = DateTimeOffset.Parse("2018-06-14 6:00:00 pm");

            var tz = TimeZone.Default;

            var defaultNow = tz.FromUtc(utcnow);
            Console.WriteLine(defaultNow);
            Assert.AreEqual("6/14/2018 2:00:00 PM -04:00", defaultNow.ToString());

            var tz2 = new TimeZone("Eastern Standard Time");
            var explicitNow = tz2.FromUtc(utcnow);
            Console.WriteLine(explicitNow);
            Assert.AreEqual("6/14/2018 2:00:00 PM -04:00", explicitNow.ToString());
        }
    }
}