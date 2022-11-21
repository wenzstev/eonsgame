using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class InfluenceActionTestSuite : CultureInteractionTest
{
    [SetUp]
    public void SetUp()
    {
        Neighbor.Init(TestTile, 1);
    }

    [Test]
    public void CanInfluenceNeighborCulture()
    {
        CultureInfluenceAction cia = new CultureInfluenceAction(TestCulture);
        Turn turn = cia.ExecuteTurn();

        Assert.That(turn.turnUpdates.ContainsKey(Neighbor), "Neighbor experiencing no changes!");

        CultureTurnUpdate NeighborUpdate = turn.turnUpdates[Neighbor];
        Assert.That(CultureHelperMethods.GetColorDistance(TestCulture.Color, NeighborUpdate.newColor) 
                    < CultureHelperMethods.GetColorDistance(TestCulture.Color, Neighbor.Color),
                    "Cultures are not closer together!"); 
    }

    [Test]
    public void CanMergeIntoNewCultureIfClose()
    {
        Turn.HookTurn().UpdateCulture(TestCulture).newColor = Color.blue;
        Turn.HookTurn().UpdateCulture(Neighbor).newColor = Color.blue;
        Turn.HookTurn().UpdateAllCultures();

        CultureInfluenceAction cia = new CultureInfluenceAction(TestCulture);
        Turn turn = cia.ExecuteTurn();

        Assert.That(turn.turnUpdates.ContainsKey(TestCulture), "Test culture experiencing no changes!");
        Assert.That(turn.turnUpdates.ContainsKey(Neighbor), "Neighbor experiencing no changes!");

        CultureTurnUpdate TestCultureUpdate = turn.turnUpdates[TestCulture];
        CultureTurnUpdate neighborUpdate = turn.turnUpdates[Neighbor];

        Assert.That(neighborUpdate.newState == Culture.State.PendingRemoval, "Neighbor not slated for removal!");
        Assert.That(TestCultureUpdate.newName != "", "Culture not getting new name!");
        Assert.That(TestCultureUpdate.popChange == Neighbor.Population, "Culture not gaining members of neighbor culture!");
    }
}
