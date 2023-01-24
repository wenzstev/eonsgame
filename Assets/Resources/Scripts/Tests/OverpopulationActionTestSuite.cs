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
        TestCulture.GetComponent<CultureFoodStore>().AlterFoodStore(25);
        yield return null;
    }

    [Test]
    public void CanBecomeOverPopulated()
    {
        TestCulture.spreadChance = 0;
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        DefaultAction.ExecuteTurn(cultureTurnInfo);

        Assert.AreEqual(Culture.State.Overpopulated, TestUtils.GetLastStateInUpdateList(Turn.CurrentTurn.UpdateHolder.GetStateUpdates(), TestCulture), "Culture not in overpopulated state!");
    }

}
