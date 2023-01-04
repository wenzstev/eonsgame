using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class CultureTestSuite : CultureInteractionTest
{
    [SetUp]
    public void SetUpInteractionTest()
    {
        GameObject TimeControllerObject = new GameObject("TimeController");
        TimeControllerObject.AddComponent<TimeController>(); // need a TimeController because the template that the culture needs to create a new culture requires it (TODO: reduce this dependency)
    }

    [Test]
    public void TestSplitCulture()
    {
        
        TestCulture.AddPopulation(4);
        int oldPopulation = TestCulture.Population;

        GameObject splitCultureObj = TestCulture.SplitCultureFromParent();
        Culture splitCulture = splitCultureObj.GetComponent<Culture>();

        Assert.That(splitCulture.name == TestCulture.name, "Split culture's name doesn't match!");
        Assert.That(splitCulture.Population >= TestCulture.minPopTransfer && splitCulture.Population <= TestCulture.maxPopTransfer, "Split culture's size doesn't match split amount!");
        Assert.That(TestCulture.Population == oldPopulation - splitCulture.Population, "Parent culture's population wasn't lowered!");
    }

    [Test]
    public void TestCannotMerge()
    {
        Neighbor.SetColor(Color.blue);
        Assert.That(!TestCulture.CanMerge(Neighbor), "CanMerge returned true when should return false!");
    }

    [Test]
    public void TestCanMerge()
    {
        Neighbor.SetColor(Color.blue);
        TestCulture.SetColor(Color.blue);
        Assert.That(TestCulture.CanMerge(Neighbor), "CanMerge returned false when should return true!");
    }
}
