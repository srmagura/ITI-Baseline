using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Domain
{
    [TestClass]
    public class ValueObjectTests
    {
        [TestMethod]
        public void SimpleEquality()
        {
            var email1 = new EmailAddress("test@example2.com");
            var email1Copy = new EmailAddress("test@example2.com");
            var email2 = new EmailAddress("test2@example2.com");

            Assert.AreSame(email1, email1);
            Assert.AreEqual(email1, email1Copy);
            Assert.IsTrue(email1 == email1Copy);
            Assert.AreNotEqual(email1, email2);
        }

        [TestMethod]
        public void MultiFieldEquality()
        {
            var name1 = new SimplePersonName("Sam", null, "Magura", "Mr.");
            var name1Copy = new SimplePersonName("Sam", null, "Magura", "Mr.");
            var name2 = new SimplePersonName("Sam", "Rosso", "Magura", "Mr.");
            var name3 = new SimplePersonName("Sam1", null, "Magura", "Mr.");

            Assert.AreEqual(name1, name1Copy);
            Assert.AreNotEqual(name1, name2);
            Assert.AreNotEqual(name1, name3);
        }

        private class Person : ValueObject
        {
            public SimplePersonName Name { get; set; }
            public int Age { get; set; }

            public Person(SimplePersonName name, int age)
            {
                Name = name;
                Age = age;
            }

            protected override IEnumerable<object?> GetAtomicValues()
            {
                yield return Name;
                yield return Age;
            }
        }

        [TestMethod]
        public void NestedEquality()
        {
            var person1 = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura", "Mr."),
                26
            );
            var person1Copy = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura", "Mr."),
                26
            );
            var person2 = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura", null),
                26
            );
            var person3 = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura", "Mr."),
                27
            );

            Assert.AreEqual(person1, person1Copy);
            Assert.AreNotEqual(person1, person2);
            Assert.AreNotEqual(person1, person3);
        }
    }
}
