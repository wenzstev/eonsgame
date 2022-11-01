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
        Turn.HookTurn().UpdateCulture(TestCulture).popChange = 100;
        Turn.HookTurn().UpdateAllCultures();
        yield return null;
    }

    [Test]
    public void CanBecomeOverPopulated()
    {
        TestCulture.spreadChance = 0;
        DefaultAction da = new DefaultAction(TestCulture);
        Turn turn = da.ExecuteTurn();
        CultureTurnUpdate cta = turn.turnUpdates[TestCulture];
        Assert.That(cta.newState == Culture.State.Overpopulated, "Culture not in overpopulated state!");
    }

    [Test]
    public void TestPopulationDrop()
    {
        OverpopulationAction opa = new OverpopulationAction(TestCulture);
        opa.popLossChance = 1f;
        Turn turn = opa.ExecuteTurn();

        CultureTurnUpdate cta = turn.turnUpdates[TestCulture];
        Assert.That(cta.popChange == -1, "Culture did not lose size!");


    }

    [Test]
    public void TestPopulationMoveAndDrop()
    {
        // if population is too high,
        // culture can attempt to flee and split off
        // if that fails, some population is killed



        OverpopulationAction opa = new OverpopulationAction(TestCulture);
        opa.popLossChance = 0f;
        Turn turn = opa.ExecuteTurn();

        CultureTurnUpdate cta = turn.turnUpdates[TestCulture];
        Assert.That(turn.turnUpdates.Count == 2, "Culture did not split!");
        Assert.That(TestCulture.Population == 10, "Culture did not lose size!");
    }
   
}
