using ITI.Baseline.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Util;

[TestClass]
public class JsonExtensionsTests
{
    private class MyObject
    {
        public string? Property { get; set; }
    }

    [TestMethod]
    public void ItConvertsToAndFromDbJson()
    {
        var originalObj = new MyObject { Property = "value" };
        var json = originalObj.ToDbJson();
        Assert.AreEqual("{\"Property\":\"value\"}", json);

        var obj = json.FromDbJson<MyObject>();
        Assert.AreEqual("value", obj?.Property);

        // Verify that property name case does not matter
        obj = json.ToLowerInvariant().FromDbJson<MyObject>();
        Assert.AreEqual("value", obj?.Property);
    }

    [TestMethod]
    public void Dump()
    {
        var obj = new MyObject { Property = "value" };

        var lines = new List<string>();
        obj.Dump(null, s => lines.Add(s));

        Assert.AreEqual(2, lines.Count);
        Assert.IsTrue(lines[0].Contains("MyObject"));
        Assert.IsTrue(lines[1].Contains("Property"));
        Assert.IsTrue(lines[1].Contains("value"));

        obj.ConsoleDump();
    }
}
