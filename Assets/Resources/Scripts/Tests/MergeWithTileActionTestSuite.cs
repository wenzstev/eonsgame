using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Linq;
public class MergeWithTileActionTestSuite : CultureInteractionTest
{


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
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);

        RenameAndRecolorCulture(Neighbor, "test", new Color(0, 0, .96f, 1));


        Color a = TestCulture.Color;
        Color b = Neighbor.Color;
        Color c = Color.Lerp(a, b, .5f); // new color will be halfway between

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(1, TestUtils.GetCombinedPopulationInUpdateList(Turn.CurrentTurn.UpdateHolder.GetIntUpdates(), TestCulture), "Merged culture's population change isn't expected!");
        Assert.AreEqual(c, TestUtils.GetLastColorInUpdateList(Turn.CurrentTurn.UpdateHolder.GetColorUpdates(), TestCulture), "Merged culture's color isn't expected!");
        Assert.AreEqual(Culture.State.PendingRemoval, TestUtils.GetLastStateInUpdateList(Turn.CurrentTurn.UpdateHolder.GetStateUpdates(), Neighbor), "Merging culture isn't slated for removal!");
        Assert.AreEqual(-1, TestUtils.GetCombinedPopulationInUpdateList(Turn.CurrentTurn.UpdateHolder.GetIntUpdates(), Neighbor), "Merged culture's population change isn't expected!");
    }

    [Test]
    public void CanAddNewCultureToTile()
    {
        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(Culture.State.Invaded, TestUtils.GetLastStateInUpdateList(Turn.CurrentTurn.UpdateHolder.GetStateUpdates(), TestCulture),"Culture on tile not set to invaded!");
        Assert.AreEqual(Culture.State.Invader, TestUtils.GetLastStateInUpdateList(Turn.CurrentTurn.UpdateHolder.GetStateUpdates(), Neighbor), "Invader hasn't been set to invade!");
    }

    [Test]
    public void CanCreateAsNewCulture()
    {
        RenameAndRecolorCulture(TestCulture, "test", Color.blue);
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);

        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);

        RenameAndRecolorCulture(Neighbor, "test", Color.red);

        cultureTurnInfo = new CultureTurnInfo(Neighbor, Turn.CurrentTurn);

        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);

        Assert.AreEqual(1, Turn.CurrentTurn.UpdateHolder.GetStringUpdates().Where(u => u.Target == Neighbor).ToArray().Length, "Culture didn't get a name change!");

        Turn.UpdateAllCultures();

        Assert.AreNotEqual("test", Neighbor.Name);

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.AreEqual(Culture.State.Invader, TestUtils.GetLastStateInUpdateList(Turn.CurrentTurn.UpdateHolder.GetStateUpdates(), Neighbor), "Culture isn't an invader!");
        Assert.AreEqual(Culture.State.Invaded, TestUtils.GetLastStateInUpdateList(Turn.CurrentTurn.UpdateHolder.GetStateUpdates(), TestCulture), "Invaded culture isn't marked as such!");
    }

    void RenameAndRecolorCulture(Culture c, string newName, Color newColor)
    {
        c.RenameCulture(newName);
        c.SetColor(newColor);
    }


    void ExecuteTurnAndSetCultureTurnUpdates()
    {
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(Neighbor, Turn.CurrentTurn);

        MergeWithTileAction.CombineCultureWithNewTile(cultureTurnInfo);
    }
}
