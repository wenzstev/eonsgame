using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Linq;
public class MergeWithTileActionTestSuite : CultureInteractionTest
{

    INonGenericCultureUpdate[] TestCultureUpdateList;
    INonGenericCultureUpdate[] NeighborUpdateList;

    [SetUp]
    public void SetUp()
    {
        Neighbor.Init(NeighborTile, 1);
        Neighbor.SetTile(TestTile, false);
    }

    [Test]
    public void CanMergeCultures()
    {
        string oldCultureName = TestCulture.name;

        RenameAndRecolorCultures("test", "test", Color.blue, new Color(0, 0, .96f, 1));

        Color a = TestCulture.Color;
        Color b = Neighbor.Color;
        Color c = Color.Lerp(a, b, .5f); // new color will be halfway between

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(1, TestUtils.GetCombinedPopulationInUpdateList(TestCultureUpdateList), "Merged culture's population change isn't expected!");
        Assert.AreEqual(c, TestUtils.GetLastColorInUpdateList(TestCultureUpdateList), "Merged culture's color isn't expected!");
        Assert.AreEqual(Culture.State.PendingRemoval, TestUtils.GetLastStateInUpdateList(NeighborUpdateList), "Merging culture isn't slated for removal!");
        Assert.AreEqual(-1, TestUtils.GetCombinedPopulationInUpdateList(NeighborUpdateList), "Merged culture's population change isn't expected!");
    }

    [Test]
    public void CanAddNewCultureToTile()
    {
        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(Culture.State.Invaded, TestUtils.GetLastStateInUpdateList(TestCultureUpdateList),"Culture on tile not set to invaded!");
        Assert.AreEqual(Culture.State.Invader, TestUtils.GetLastStateInUpdateList(NeighborUpdateList), "Invader hasn't been set to invade!");
    }

    [Test]
    public void CanCreateAsNewCulture()
    {
        RenameAndRecolorCultures("test", "test", Color.blue, Color.red);

        ExecuteTurnAndSetCultureTurnUpdates();
        Assert.AreEqual(1, NeighborUpdateList.Where(u => u.GetType() == typeof(NameUpdate)).ToArray().Length, "Culture didn't get a name change!");

        Turn.UpdateAllCultures();

        Assert.AreNotEqual("test", Neighbor.Name);

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(Culture.State.Invader, TestUtils.GetLastStateInUpdateList(NeighborUpdateList), "Culture isn't an invader!");
        Assert.AreEqual(Culture.State.Invaded, TestUtils.GetLastStateInUpdateList(TestCultureUpdateList), "Invaded culture isn't marked as such!");
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
        MergeWithTileAction testMergeAction = new MergeWithTileAction(Neighbor);
        testMergeAction.ExecuteTurn();

        TestCultureUpdateList = Turn.GetPendingUpdatesFor(TestCulture);
        NeighborUpdateList = Turn.GetPendingUpdatesFor(Neighbor);
    }
}
