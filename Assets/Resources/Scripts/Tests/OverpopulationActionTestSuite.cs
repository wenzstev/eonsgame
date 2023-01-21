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
<<<<<<< HEAD
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        DefaultAction.ExecuteTurn(cultureTurnInfo);

=======
        DefaultAction da = new DefaultAction(TestCulture);
        da.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        INonGenericCultureUpdate[] TestCultureUpdates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(Culture.State.Overpopulated, TestUtils.GetLastStateInUpdateList(TestCultureUpdates), "Culture not in overpopulated state!");
    }

    [Test]
    public void TestPopulationDrop()
    {
<<<<<<< HEAD
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        OverpopulationAction.ExecuteTurn(cultureTurnInfo);
=======
        OverpopulationAction opa = new OverpopulationAction(TestCulture);
        opa.popLossChance = 1f;
        opa.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

        INonGenericCultureUpdate[] TestCultureUpdates = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(-1, TestUtils.GetCombinedPopulationInUpdateList(TestCultureUpdates), "Culture did not lose size!");
    }
}
