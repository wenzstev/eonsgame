using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Linq;
public class MergeActionTestSuite : CultureInteractionTest
{

    INonGenericCultureUpdate[] TestCultureUpdateList;
    INonGenericCultureUpdate[] NeighborUpdateList;

    [SetUp]
    public void SetUp()
    {
        TestCulture.Init(TestTile, 1);
        Neighbor.transform.parent = TestTile.transform;
    }

    [Test]
    public void CanMergeCultures()
    {
        string oldCultureName = TestCulture.name;

        RenameAndRecolorCultures("test", "test", Color.blue, new Color(0, 0, .96f, 1));

        TestCulture.tileInfo.UpdateCultureName(oldCultureName, "test");

        Color a = TestCulture.Color;
        Color b = Neighbor.Color;
        Color c = Color.Lerp(a, b, .5f); // new color will be halfway between

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(1, TestUtils.GetCombinedPopulationInUpdateList(TestCultureUpdateList), "Merged culture's population change isn't expected!");
        Assert.AreEqual(c, TestUtils.GetLastColorInUpdateList(TestCultureUpdateList), "Merged culture's color isn't expected!");
        Assert.AreEqual(Culture.State.PendingRemoval, TestUtils.GetLastStateInUpdateList(NeighborUpdateList), "Merging culture isn't slated for removal!");
        Assert.AreEqual(0, TestUtils.GetCombinedPopulationInUpdateList(NeighborUpdateList), "Merged culture's population change isn't expected!");
    }

    [Test]
    public void CanAddNewCultureToTile()
    {
        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(Culture.State.Invaded, TestUtils.GetLastStateInUpdateList(TestCultureUpdateList),"Culture on tile not set to invaded!");
        Assert.AreEqual(Culture.State.Invader, TestUtils.GetLastStateInUpdateList(NeighborUpdateList), "Invader hasn't been set to invade!");
        Assert.AreEqual(TestTile, TestUtils.GetLastTileInUpdateList(NeighborUpdateList), "New Culture isn't moving to tile!");
    }

    [Test]
    public void CanMergeWithParentCulture()
    {
        string oldCultureName = TestCulture.name;

        RenameAndRecolorCultures("test", "test", Color.blue, new Color(0, 0, .96f, 1));

        Neighbor.RenameCulture("test2");
        
        TestCulture.tileInfo.UpdateCultureName(oldCultureName, "test");

        Color c = Color.Lerp(TestCulture.Color, Neighbor.Color, .5f); // new color will be halfway between

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(1, TestUtils.GetCombinedPopulationInUpdateList(NeighborUpdateList), "Merged Culture didn't have it's population changed!");
        Assert.AreEqual(c, TestUtils.GetLastColorInUpdateList(NeighborUpdateList), "Merged Culture didn't have it's color changed!");
        Assert.AreEqual(Culture.State.PendingRemoval, TestUtils.GetLastStateInUpdateList(TestCultureUpdateList), "Merging culture isn't slated for removal!");
        Assert.AreEqual(-TestCulture.Population, TestUtils.GetCombinedPopulationInUpdateList(TestCultureUpdateList), "Merging culture doesn't have pop set to zero!");
        Assert.AreEqual( TestTile, TestUtils.GetLastTileInUpdateList(NeighborUpdateList), "Merged culture isn't moving to tile!");

    }

    [Test]
    public void CanCreateAsNewCulture()
    {
        RenameAndRecolorCultures("test", "test", Color.blue, Color.red);

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(1, NeighborUpdateList.Where(u => u.GetType() == typeof(NameUpdate)).ToArray().Length, "Culture didn't get a name change!");
        Assert.AreEqual(Culture.State.Invader, TestUtils.GetLastStateInUpdateList(NeighborUpdateList), "Culture isn't an invader!");
        Assert.AreEqual(Culture.State.Invaded, TestUtils.GetLastStateInUpdateList(TestCultureUpdateList), "Invaded culture isn't marked as such!");
        Assert.AreEqual(TestTile, TestUtils.GetLastTileInUpdateList(NeighborUpdateList), "New Culture hasn't moved to tile!");
    }

    void RenameAndRecolorCultures(string testCultureName, string neighborName, Color testCultureColor, Color neighborColor)
    {
        TestCulture.RenameCulture(testCultureName);
        Neighbor.RenameCulture(neighborName);
        TestCulture.SetColor(testCultureColor);
        Neighbor.SetColor(neighborColor);
    }

    void ExecuteTurnAndSetCultureTurnUpdates()
    {
        MergeAction testMergeAction = new MergeAction(Neighbor);
        testMergeAction.ExecuteTurn();

        TestCultureUpdateList = Turn.GetPendingUpdatesFor(TestCulture);
        NeighborUpdateList = Turn.GetPendingUpdatesFor(Neighbor);

        Assert.AreEqual(1, TestCultureUpdateList.Length, "Update list is not expected length!");
        Assert.AreEqual(1, NeighborUpdateList.Length, "Update list is not expected length!");
    }
}
