using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class OverpopulationActionTestSuite : CultureActionTest
{

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        TestCulture.AddPopulation(100);
        yield return null;
    }

    [Test]
    public void CanBecomeOverPopulated()
    {
        TestCulture.spreadChance = 0;
        DefaultAction da = new DefaultAction(TestCulture);
        da.ExecuteTurn();
        INonGenericCultureUpdate[] TestCultureUpdates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(Culture.State.Overpopulated, TestUtils.GetLastStateInUpdateList(TestCultureUpdates), "Culture not in overpopulated state!");
    }

    [Test]
    public void TestPopulationDrop()
    {
        OverpopulationAction opa = new OverpopulationAction(TestCulture);
        opa.popLossChance = 1f;
        opa.ExecuteTurn();

        INonGenericCultureUpdate[] TestCultureUpdates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(-1, TestUtils.GetCombinedPopulationInUpdateList(TestCultureUpdates), "Culture did not lose size!");
    }
}
