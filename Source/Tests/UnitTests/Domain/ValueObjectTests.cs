using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Domain.ValueObjects;

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
            var name1 = new SimplePersonName("Sam", null, "Magura");
            var name1Copy = new SimplePersonName("Sam", null, "Magura");
            var name2 = new SimplePersonName("Sam", "Rosso", "Magura");
            var name3 = new SimplePersonName("Sam1", null, "Magura");

            Assert.AreEqual(name1, name1Copy);
            Assert.AreNotEqual(name1, name2);
            Assert.AreNotEqual(name1, name3);
        }

        private record Person : DbValueObject
        {
            public SimplePersonName Name { get; set; }
            public int Age { get; set; }

            public Person(SimplePersonName name, int age)
            {
                Name = name;
                Age = age;
            }
        }

        [TestMethod]
        public void NestedEquality()
        {
            var person1 = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura"),
                26
            );
            var person1Copy = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura"),
                26
            );
            var person2 = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura1"),
                26
            );
            var person3 = new Person(
                new SimplePersonName("Sam", "Rosso", "Magura"),
                27
            );

            Assert.AreEqual(person1, person1Copy);
            Assert.AreNotEqual(person1, person2);
            Assert.AreNotEqual(person1, person3);
        }
    }
}
