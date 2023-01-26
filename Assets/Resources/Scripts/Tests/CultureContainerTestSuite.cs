using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class CultureContainerTestSuite : BasicTest
{
    Culture TestCultureA;
    Culture TestCultureB;
    CultureContainer TestCultureContainer;

    [SetUp]
    public void SetUpCultureContainerTest()
    {
        GameObject TestCultureAObj = new GameObject("CultureA");
        GameObject TestCultureBObj = new GameObject("CultureB");

        TestCultureA = TestCultureAObj.AddComponent<Culture>();
        TestCultureB = TestCultureBObj.AddComponent<Culture>();

        TestCultureA.RenameCulture("CultureA");
        TestCultureB.RenameCulture("CultureB");

        GameObject TestCultureContainerObj = new GameObject("TestCultureContainer");
        TestCultureContainer = TestCultureContainerObj.AddComponent<CultureContainer>();
        TestCultureContainer.Initialize();
    }

    [Test]
    public void CanAddCultureToContainer()
    {
        TestCultureContainer.AddCulture(TestCultureA);
        Assert.AreEqual(1, TestCultureContainer.GetAllCultures().Count, "CultureContainer does not have expected number of cultures!");
    }

    [Test]
    public void CanSortCultures()
    {
        TestCultureA.AddPopulation(5);
        TestCultureB.AddPopulation(2);

        Assert.AreEqual(5, TestCultureA.Population);
        Assert.AreEqual(2, TestCultureB.Population);

        TestCultureContainer.AddCulture(TestCultureB);
        TestCultureContainer.AddCulture(TestCultureA);

        Assert.AreEqual(TestCultureA, TestCultureContainer.GetAllCultures()[0], "CultureA should be first!");
    }

    [Test]
    public void CanResortWhenPopulationAdded()
    {
        TestCultureA.AddPopulation(5);
        TestCultureB.AddPopulation(2);

        TestCultureContainer.AddCulture(TestCultureA);
        TestCultureContainer.AddCulture(TestCultureB);

        TestCultureB.AddPopulation(10);
        Assert.AreEqual(TestCultureB, TestCultureContainer.GetAllCultures()[0], "CultureB should be first!");
    }

    [UnityTest]
    public IEnumerator CanResortWhenCultureDestroyed()
    {
        TestCultureA.AddPopulation(5);
        TestCultureB.AddPopulation(2);


        TestCultureContainer.AddCulture(TestCultureA);
        TestCultureContainer.AddCulture(TestCultureB);

        TestCultureA.DestroyCulture();

        yield return null;

        Assert.AreEqual(TestCultureB, TestCultureContainer.GetAllCultures()[0]);
        Assert.AreEqual(1, TestCultureContainer.GetAllCultures().Count);
    }

    [TearDown]
    public void TearDownCultureCOntainerTest()
    {
        TestUtils.TearDownTest();
    }

}
