﻿using ITI.Baseline.Passwords;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Passwords;

[TestClass]
public class DefaultPasswordValidatorTests
{
    [TestMethod]
    public void ItValidatesPasswords()
    {
        var validator = new DefaultPasswordValidator();

        Assert.IsTrue(validator.IsValid("LetMeIn98"));
        Assert.IsTrue(validator.IsValid("LetMeIn98@&*"));
        Assert.IsTrue(validator.IsValid("Let Me In 98"));

        Assert.IsFalse(validator.IsValid("Let98"));
        Assert.IsFalse(validator.IsValid(" LetMeIn98 "));
        Assert.IsFalse(validator.IsValid("LetMeIn98\t"));
        Assert.IsFalse(validator.IsValid("LetMeInForReal"));
        Assert.IsFalse(validator.IsValid("letmeinforreal"));
    }
}
