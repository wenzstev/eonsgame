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

        GameObject TestPopulationPipMakerObj = new GameObject("Population Pip Maker");
        TestPopulationPipMaker = TestPopulationPipMakerObj.AddComponent<PopulationPipMaker>();
        TestPopulationPipMaker.PopPip = Resources.Load<GameObject>("Prefabs/Board/Inhabitants/PopulationPip");
        TestPopulationPipMakerObj.transform.SetParent(TestCulture.transform);

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
