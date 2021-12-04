using ITI.Baseline.Passwords;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Passwords;

[TestClass]
public class Pkbdf2PasswordEncoderTests
{
    [TestMethod]
    public void EncodePassword()
    {
        var encoder = new Pkbdf2PasswordEncoder();

        var encoded = encoder.Encode("LetMeIn98");
        Console.WriteLine(encoded);

        Assert.IsTrue(encoder.IsCorrect("LetMeIn98", encoded));
        Assert.IsFalse(encoder.IsCorrect(" LetMeIn98", encoded));
        Assert.IsFalse(encoder.IsCorrect("LetMeIn99", encoded));
    }
}
