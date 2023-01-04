using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class PopulationChangeIndicatorTestSuite : CultureActionTest
{
    PopulationChangeIndicatorGenerator TestPopulationChangeIndicatorGenerator;

    [UnitySetUp]
    public IEnumerator PopulationChangeIndicatorSetUp()
    {
        GameObject TestPopulationChangeIndicatorGeneratorObj = new GameObject("Population Indicator Generator");
        TestPopulationChangeIndicatorGenerator = TestPopulationChangeIndicatorGeneratorObj.AddComponent<PopulationChangeIndicatorGenerator>();
        TestPopulationChangeIndicatorGenerator.PopulationChangeIndicator = Resources.Load<GameObject>("Prefabs/Board/Inhabitants/PopulationChangeIndicator");
        TestPopulationChangeIndicatorGeneratorObj.transform.SetParent(TestCulture.transform);
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanGenerateIndicator()
    {
        TestCulture.AddPopulation(1);
        yield return null;

        // get the Populationchangeindicator and then check if it has a child

        Assert.AreEqual(1, TestPopulationChangeIndicatorGenerator.transform.childCount, "Population Indicator was not generated!");

    }

}
