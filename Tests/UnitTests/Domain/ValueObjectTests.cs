using ITI.DDD.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Domain.ValueObjects;

namespace UnitTests.Domain;

[TestClass]
public class ValueObjectTests
{
    private record Person : DbValueObject
    {
        public PersonName Name { get; set; }
        public int Age { get; set; }

        public Person(PersonName name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    [TestMethod]
    public void NestedEquality()
    {
        var person1 = new Person(
            new PersonName("Sam", "Rosso", "Magura"),
            26
        );
        var person1Copy = new Person(
            new PersonName("Sam", "Rosso", "Magura"),
            26
        );
        var person2 = new Person(
            new PersonName("Sam", "Rosso", "Magura1"),
            26
        );
        var person3 = new Person(
            new PersonName("Sam", "Rosso", "Magura"),
            27
        );

        Assert.AreEqual(person1, person1Copy);
        Assert.AreNotEqual(person1, person2);
        Assert.AreNotEqual(person1, person3);
    }
}
