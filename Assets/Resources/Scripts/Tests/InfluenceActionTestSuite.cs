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
<<<<<<< HEAD
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        CultureInfluenceAction.ExecuteTurn(cultureTurnInfo);
=======
        CultureInfluenceAction cia = new CultureInfluenceAction(TestCulture);
        cia.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

        INonGenericCultureUpdate[] NeighborList = Turn.GetPendingUpdatesFor(Neighbor);
        
        Assert.That(NeighborList.Length > 0, "Neighbor experiencing no changes!");

        Color newColor = TestUtils.GetLastColorInUpdateList(NeighborList);

        Assert.That(CultureHelperMethods.GetColorDistance(TestCulture.Color, newColor) 
                    < CultureHelperMethods.GetColorDistance(TestCulture.Color, Neighbor.Color),
                    "Cultures are not closer together!"); 
    }

    [Test]
    public void CanMergeIntoNewCultureIfClose()
    {
        TestCulture.SetColor(Color.blue);
        Neighbor.SetColor(Color.blue);

<<<<<<< HEAD
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        CultureInfluenceAction.ExecuteTurn(cultureTurnInfo);
=======

        CultureInfluenceAction cia = new CultureInfluenceAction(TestCulture);
        cia.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

        INonGenericCultureUpdate[] NeighborList = Turn.GetPendingUpdatesFor(Neighbor);
        INonGenericCultureUpdate[] TestCultureList = Turn.GetPendingUpdatesFor(TestCulture);


        Assert.That(NeighborList.Length > 0, "Neighbor experiencing no changes!");
        Assert.That(TestCultureList.Length > 0, "Neighbor experiencing no changes!");

        Assert.AreEqual(Culture.State.PendingRemoval, TestUtils.GetLastStateInUpdateList(NeighborList), "Neighbor not slated for removal!");
        Assert.AreEqual(1, TestCultureList.Where(u => u.GetCultureChange().GetType() == typeof(string)).ToArray().Length, "Culture not getting new name!");
        Assert.AreEqual(Neighbor.Population, TestUtils.GetCombinedPopulationInUpdateList(TestCultureList), "Culture not gaining members of neighbor culture!");
    }
}
