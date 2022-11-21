using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class PopulationPipMakerTestSuite : CultureActionTest
{
    PopulationPipMaker TestPopulationPipMaker;

    [UnitySetUp]
    public IEnumerator SetUpWithRightPopulation()
    {
        TestCulture.Init(TestTile, 4);
        TestPopulationPipMaker = TestCulture.GetComponentInChildren<PopulationPipMaker>();
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanAddPip()
    {
        Assert.AreEqual(0, TestPopulationPipMaker.transform.childCount, "Culture did not start with the right number of pips!");
        TestCulture.AddPopulation(1);
        EventManager.TriggerEvent("Tick", null);
        yield return null;
        Assert.AreEqual(1, TestPopulationPipMaker.transform.childCount, "Culture does not have the right number of pips!");
    }


    [UnityTest]
    public IEnumerator CanRemovePip()
    {
        TestCulture.AddPopulation(15);
        EventManager.TriggerEvent("Tick", null);

        yield return null;
        TestCulture.AddPopulation(-1);
        EventManager.TriggerEvent("Tick", null);

        yield return null;
        Assert.AreEqual(2, TestPopulationPipMaker.transform.childCount, "Culture did not reduce pips!");
    }

}
