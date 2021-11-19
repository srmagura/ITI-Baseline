using ITI.Baseline.Passwords;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests.Passwords
{
    [TestClass]
    public class DefaultPasswordEncoderTests
    {
        [TestMethod]
        public void EncodePassword()
        {
            var settings = new DefaultPasswordEncoderSettings() { Pbkdf2Iterations = 3 };
            var encoder = new DefaultPasswordEncoder(settings);

            var isValid = encoder.IsValid("LetMeIn98");
            Assert.IsTrue(isValid);

            var enc = encoder.Encode("LetMeIn98");
            Console.WriteLine(enc);

            Assert.IsTrue(encoder.IsCorrect("LetMeIn98", enc));
            Assert.IsFalse(encoder.IsCorrect("LetMeIn99", enc));
        }
    }
}
