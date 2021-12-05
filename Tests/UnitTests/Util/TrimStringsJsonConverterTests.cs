using System.Text.Json;
using ITI.Baseline.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Util;

[TestClass]
public class TrimStringsJsonConverterTests
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new TrimStringsJsonConverter() }
    };

    [TestMethod]
    public void String()
    {
        Assert.AreEqual("test", JsonSerializer.Deserialize<string>("\" test \"", SerializerOptions));
    }

    [TestMethod]
    public void ListOfStrings()
    {
        var originalList = new List<string> { "test1 ", "test2", " test3" };
        var originalListJson = JsonSerializer.Serialize(originalList);

        var list = JsonSerializer.Deserialize<List<string>>(originalListJson, SerializerOptions);
        Assert.IsNotNull(list);

        for (var i = 0; i < originalList.Count; i++)
        {
            Assert.AreEqual(originalList[i].Trim(), list[i]);
        }
    }

    private class MyObject
    {
        public string? MyProperty1 { get; set; }
        public string? MyProperty2 { get; set; }
    }

    [TestMethod]
    public void Object()
    {
        var originalObject = new MyObject { MyProperty1 = " test " };
        var originalObjectJson = JsonSerializer.Serialize(originalObject);
        var obj = JsonSerializer.Deserialize<MyObject>(originalObjectJson, SerializerOptions);

        Assert.IsNotNull(obj);
        Assert.AreEqual("test", obj.MyProperty1);
        Assert.IsNull(obj.MyProperty2);
    }
}
