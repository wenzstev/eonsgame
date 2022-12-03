using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
public class MergeActionTestSuite : CultureInteractionTest
{
    CultureTurnUpdate TestCultureUpdate;
    CultureTurnUpdate NeighborUpdate;

    
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

        Assert.That(TestCultureUpdate.popChange == 1, "Merged Culture didn't have it's population changed!");
        Assert.AreEqual(c, TestCultureUpdate.newColor, "Merged Culture didn't have it's color changed!");
        Assert.That(NeighborUpdate.newState == Culture.State.PendingRemoval, "Merging culture isn't slated for removal!");
        Assert.That(NeighborUpdate.popChange == -Neighbor.Population, "Merging culture doesn't have pop set to zero!");
    }

    [Test]
    public void CanAddNewCultureToTile()
    {
        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.That(TestCultureUpdate.newState == Culture.State.Invaded, "Culture on tile not set to invaded!");
        Assert.That(NeighborUpdate.newState == Culture.State.Invader, "Invader hasn't been set to invade!");
        Assert.That(NeighborUpdate.newTile == TestTile, "New Culture isn't moving to tile!");
    }

    [Test]
    public void CanMergeWithParentCulture()
    {
        string oldCultureName = TestCulture.name;

        RenameAndRecolorCultures("test", "test", Color.blue, new Color(0, 0, .96f, 1));

        Turn.HookTurn().UpdateCulture(Neighbor).newName = "test2";
        Turn.HookTurn().UpdateAllCultures();

        TestCulture.tileInfo.UpdateCultureName(oldCultureName, "test");

        Color c = Color.Lerp(TestCulture.Color, Neighbor.Color, .5f); // new color will be halfway between

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.That(NeighborUpdate.popChange == 1, "Merged Culture didn't have it's population changed!");
        Assert.That(NeighborUpdate.newColor == c, "Merged Culture didn't have it's color changed!");
        Assert.That(TestCultureUpdate.newState == Culture.State.PendingRemoval, "Merging culture isn't slated for removal!");
        Assert.That(TestCultureUpdate.popChange == -TestCulture.Population, "Merging culture doesn't have pop set to zero!");
        Assert.That(NeighborUpdate.newTile == TestTile, "Merged culture isn't moving to tile!");

    }

    [Test]
    public void CanCreateAsNewCulture()
    {
        RenameAndRecolorCultures("test", "test", Color.blue, Color.red);

        ExecuteTurnAndSetCultureTurnUpdates();

        Assert.That(NeighborUpdate.newName != "", "Culture didn't get a name change!");
        Assert.That(NeighborUpdate.newState == Culture.State.Invader, "Culture isn't an invader!");
        Assert.That(TestCultureUpdate.newState == Culture.State.Invaded, "Invaded culture isn't marked as such!");
        Assert.That(NeighborUpdate.newTile == TestTile, "New Culture hasn't moved to tile!");
    }

    void RenameAndRecolorCultures(string testCultureName, string neighborName, Color testCultureColor, Color neighborColor)
    {
        Turn.HookTurn().UpdateCulture(TestCulture).newName = testCultureName;
        Turn.HookTurn().UpdateCulture(Neighbor).newName = neighborName;
        Turn.HookTurn().UpdateCulture(TestCulture).newColor = testCultureColor;
        Turn.HookTurn().UpdateCulture(Neighbor).newColor = neighborColor;
        Turn.HookTurn().UpdateAllCultures();
    }

    void ExecuteTurnAndSetCultureTurnUpdates()
    {
        MergeAction testMergeAction = new MergeAction(Neighbor);
        Turn mergeTurn = testMergeAction.ExecuteTurn();

        TestCultureUpdate = mergeTurn.turnUpdates[TestCulture];
        NeighborUpdate = mergeTurn.turnUpdates[Neighbor];

        Assert.That(TestCultureUpdate != null, "Turn did not update test culture!");
        Assert.That(NeighborUpdate != null, "Turn did not update neighbor!");
    }
}
