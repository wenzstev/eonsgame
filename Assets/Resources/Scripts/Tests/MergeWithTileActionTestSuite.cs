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
        RenameAndRecolorCulture(TestCulture, "test", Color.blue);
<<<<<<< HEAD
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);
=======
        MergeWithTileAction firstCultureMerge = new MergeWithTileAction(TestCulture);
        firstCultureMerge.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

        RenameAndRecolorCulture(Neighbor, "test", new Color(0, 0, .96f, 1));


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
        RenameAndRecolorCulture(TestCulture, "test", Color.blue);
<<<<<<< HEAD
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);

        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);

        RenameAndRecolorCulture(Neighbor, "test", Color.red);

        cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);

        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);
=======
        MergeWithTileAction firstCultureMerge = new MergeWithTileAction(TestCulture);
        firstCultureMerge.ExecuteTurn();

        RenameAndRecolorCulture(Neighbor, "test", Color.red);

        MergeWithTileAction secondCultureMerge = new MergeWithTileAction(Neighbor);
        secondCultureMerge.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

        TestCultureUpdateList = Turn.GetPendingUpdatesFor(TestCulture);
        NeighborUpdateList = Turn.GetPendingUpdatesFor(Neighbor);

        Assert.AreEqual(1, NeighborUpdateList.Where(u => u.GetCultureChange().GetType() == typeof(string)).ToArray().Length, "Culture didn't get a name change!");

        Turn.UpdateAllCultures();

        Assert.AreNotEqual("test", Neighbor.Name);

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(Culture.State.Invader, TestUtils.GetLastStateInUpdateList(NeighborUpdateList), "Culture isn't an invader!");
        Assert.AreEqual(Culture.State.Invaded, TestUtils.GetLastStateInUpdateList(TestCultureUpdateList), "Invaded culture isn't marked as such!");
    }

    void RenameAndRecolorCulture(Culture c, string newName, Color newColor)
    {
        c.RenameCulture(newName);
        c.SetColor(newColor);
    }


    void ExecuteTurnAndSetCultureTurnUpdates()
    {
<<<<<<< HEAD
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);

        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);
=======
        MergeWithTileAction testMergeAction = new MergeWithTileAction(Neighbor);
        testMergeAction.ExecuteTurn();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

        TestCultureUpdateList = Turn.GetPendingUpdatesFor(TestCulture);
        NeighborUpdateList = Turn.GetPendingUpdatesFor(Neighbor);
    }
}
