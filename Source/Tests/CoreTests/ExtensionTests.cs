using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Core.DateTime;
using Iti.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void ListDelim()
        {
            var list = new List<int> { 1, 2, 3, 4 };
            var s = list.ToDelimited("|");
            Console.WriteLine(s);
            Assert.AreEqual("|1|2|3|4|", s);

            s = list.ToDelimited("|", false);
            Console.WriteLine(s);
            Assert.AreEqual("1|2|3|4", s);

            list = new List<int>();
            s = list.ToDelimited("|");
            Console.WriteLine(s);
            Assert.AreEqual("", s);

            s = list.ToDelimited("|", false);
            Console.WriteLine(s);
            Assert.AreEqual("", s);

        }

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
        public void EasternTime()
        {
            var s = TimeZoneInfo.Local.Id;
            Console.WriteLine(s);

            s = DateTimeService.DefaultTimeZoneId;
            Console.WriteLine(s);

            var tz = DateTimeService.Lookup("Eastern Standard Time");
            Console.WriteLine(tz.Id);
        }

        [TestMethod]
        public void TimeZoneUtcConvert()
        {
            var utcnow = DateTimeOffset.Parse("2018-06-14 6:00:00 pm");

            var defaultNow = DateTimeService.FromUtc(DateTimeService.DefaultTimeZoneId, utcnow);
            Console.WriteLine(defaultNow);
            Assert.AreEqual("6/14/2018 2:00:00 PM -04:00", defaultNow.ToString());

            var explicitNow = DateTimeService.FromUtc("Eastern Standard Time", utcnow);
            Console.WriteLine(explicitNow);
            Assert.AreEqual("6/14/2018 2:00:00 PM -04:00", explicitNow.ToString());
        }
    }
}