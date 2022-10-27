using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
public class MergeActionTestSuite 
{
    Culture testCulture;
    Culture testCultureMerge;
    Tile testTile;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager"));

        GameObject testTileObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Tile"));
        testTile = testTileObj.GetComponent<Tile>();

        GameObject testCultureObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        testCulture = testCultureObj.GetComponent<Culture>();

        testCulture.Init(testTile);

        GameObject testCultureMergeObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        testCultureMerge = testCultureMergeObj.GetComponent<Culture>();
        testCultureMerge.Init(Tile.moveTile.GetComponent<Tile>());
        testCultureMerge.transform.parent = testTile.transform;



        yield return null;

    }

    [Test]
    public void CanMergeCultures()
    {
        string oldCultureName = testCulture.name;
        Turn.HookTurn().UpdateCulture(testCulture).newName = "test";
        Turn.HookTurn().UpdateCulture(testCultureMerge).newName = "test";
        Turn.HookTurn().UpdateCulture(testCulture).newColor = Color.blue;
        Turn.HookTurn().UpdateCulture(testCultureMerge).newColor = new Color(0, 0, .96f, 1);

        Turn.HookTurn().UpdateAllCultures();

        testCulture.tileInfo.UpdateCultureName(oldCultureName, "test");

        Debug.Log(testTile.GetComponent<TileInfo>().cultures["test"]);


        Color a = testCulture.Color;
        Color b = testCultureMerge.Color;

        Color c = Color.Lerp(a, b, .5f); // new color will be halfway between

        MergeAction testMerge = new MergeAction(testCultureMerge);
        Turn newTurn = testMerge.ExecuteTurn();

        CultureTurnUpdate newInfoTestCulture = newTurn.turnUpdates[testCulture];
        CultureTurnUpdate newInfoMergedCulture = newTurn.turnUpdates[testCultureMerge];

        Assert.That(newInfoTestCulture.popChange == 1, "Merged Culture didn't have it's population changed!");
        Assert.That(newInfoTestCulture.newColor == c, "Merged Culture didn't have it's color changed!");
        Assert.That(newInfoMergedCulture.newState == Culture.State.PendingRemoval, "Merging culture isn't slated for removal!");
        Assert.That(newInfoMergedCulture.popChange == -testCultureMerge.Population, "Merging culture doesn't have pop set to zero!");
    }

    [Test]
    public void CanAddNewCultureToTile()
    {
        MergeAction testMergeAction = new MergeAction(testCultureMerge);
        Turn mergeTurn = testMergeAction.ExecuteTurn();

        CultureTurnUpdate newInfoTestCulture = mergeTurn.turnUpdates[testCulture];
        CultureTurnUpdate newInfoMergeCulture = mergeTurn.turnUpdates[testCultureMerge];

        Assert.That(newInfoTestCulture.newState == Culture.State.Invaded, "Culture on tile not set to invaded!");
        Assert.That(newInfoMergeCulture.newState == Culture.State.Invader, "Invader hasn't been set to invade!");
        Assert.That(newInfoMergeCulture.newTile == testTile, "New Culture isn't moving to tile!");



    }

    [Test]
    public void CanMergeWithParentCulture()
    {
        string oldCultureName = testCulture.name;
        Turn.HookTurn().UpdateCulture(testCulture).newName = "test";
        Turn.HookTurn().UpdateCulture(testCultureMerge).newName = "test";
        Turn.HookTurn().UpdateCulture(testCulture).newColor = Color.blue;
        Turn.HookTurn().UpdateCulture(testCultureMerge).newColor = new Color(0, 0, .96f, 1);
        Turn.HookTurn().UpdateAllCultures();

        Turn.HookTurn().UpdateCulture(testCultureMerge).newName = "test2";
        Turn.HookTurn().UpdateAllCultures();

        testCulture.tileInfo.UpdateCultureName(oldCultureName, "test");


        Color c = Color.Lerp(testCulture.Color, testCultureMerge.Color, .5f); // new color will be halfway between


        MergeAction testMergeAction = new MergeAction(testCultureMerge);
        Turn mergeTurn = testMergeAction.ExecuteTurn();
        

        CultureTurnUpdate newInfoTestCulture = mergeTurn.turnUpdates[testCulture];
        CultureTurnUpdate newInfoMergeCulture = mergeTurn.turnUpdates[testCultureMerge];

        Assert.That(newInfoMergeCulture.popChange == 1, "Merged Culture didn't have it's population changed!");
        Assert.That(newInfoMergeCulture.newColor == c, "Merged Culture didn't have it's color changed!");
        Assert.That(newInfoTestCulture.newState == Culture.State.PendingRemoval, "Merging culture isn't slated for removal!");
        Assert.That(newInfoTestCulture.popChange == -testCulture.Population, "Merging culture doesn't have pop set to zero!");
        Assert.That(newInfoMergeCulture.newTile == testTile, "Merged culture isn't moving to tile!");

    }

    [Test]
    public void CanCreateAsNewCulture()
    {
        Turn.HookTurn().UpdateCulture(testCulture).newName = "test";
        Turn.HookTurn().UpdateCulture(testCultureMerge).newName = "test";
        Turn.HookTurn().UpdateCulture(testCulture).newColor = Color.blue;
        Turn.HookTurn().UpdateCulture(testCultureMerge).newColor = Color.red;
        Turn.HookTurn().UpdateAllCultures();

        MergeAction testMergeAction = new MergeAction(testCultureMerge);
        Turn mergeTurn = testMergeAction.ExecuteTurn();

        CultureTurnUpdate newInfoTestCulture = mergeTurn.turnUpdates[testCulture];
        CultureTurnUpdate newInfoMergeCulture = mergeTurn.turnUpdates[testCultureMerge];

        Assert.That(newInfoMergeCulture.newName != "", "Culture didn't get a name change!");
        Assert.That(newInfoMergeCulture.newState == Culture.State.Invader, "Culture isn't an invader!");
        Assert.That(newInfoTestCulture.newState == Culture.State.Invaded, "Invaded culture isn't marked as such!");
        Assert.That(newInfoMergeCulture.newTile == testTile, "New Culture hasn't moved to tile!");
    }

    [TearDown]
    public void TearDown()
    {
        Turn.HookTurn().UpdateAllCultures();
        Debug.Log("tearing down");
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }

}
