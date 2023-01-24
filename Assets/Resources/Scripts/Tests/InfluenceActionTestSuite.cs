using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Linq;

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
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        CultureInfluenceAction.ExecuteTurn(cultureTurnInfo);

        

        Color newColor = TestUtils.GetLastColorInUpdateList(Turn.CurrentTurn.UpdateHolder.GetColorUpdates(), Neighbor);

        Assert.That(CultureHelperMethods.GetColorDistance(TestCulture.Color, newColor) 
                    < CultureHelperMethods.GetColorDistance(TestCulture.Color, Neighbor.Color),
                    "Cultures are not closer together!"); 
    }

    [Test]
    public void CanMergeIntoNewCultureIfClose()
    {
        TestCulture.SetColor(Color.blue);
        Neighbor.SetColor(Color.blue);

        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        CultureInfluenceAction.ExecuteTurn(cultureTurnInfo);


        Assert.AreEqual(Culture.State.PendingRemoval, TestUtils.GetLastStateInUpdateList(Turn.CurrentTurn.UpdateHolder.GetStateUpdates(), Neighbor), "Neighbor not slated for removal!");
        Assert.AreEqual(1, Turn.CurrentTurn.UpdateHolder.GetStringUpdates().Where(u => u.Target == TestCulture).ToArray().Length, "Culture not getting new name!");
        Assert.AreEqual(Neighbor.Population, TestUtils.GetCombinedPopulationInUpdateList(Turn.CurrentTurn.UpdateHolder.GetIntUpdates(), TestCulture), "Culture not gaining members of neighbor culture!");
    }
}
