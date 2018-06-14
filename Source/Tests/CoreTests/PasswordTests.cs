using System;
using Iti.Passwords;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        public void EncodePassword()
        {
            var pw = new DefaultPasswordEncoder();

            var isValid = pw.IsValid("LetMeIn98");
            Assert.IsTrue(isValid);

            var enc = pw.Encode("LetMeIn98");
            Console.WriteLine(enc);

            var validated = pw.Validate("LetMeIn98", enc);
            Assert.IsTrue(validated);
        }
    }
}