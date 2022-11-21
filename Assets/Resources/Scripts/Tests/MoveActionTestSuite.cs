using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class MoveActionTestSuite : CultureActionTest
{
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        NeighborTile.GetComponent<TileDrawer>().tileType = TileDrawer.BiomeType.Grassland; // otherwise the minimum height will always be underwater
        yield return null;
    }


    [UnityTest]
    public IEnumerator MoveTileActionTest()
    {

        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.moveChance = 1;
        Turn TestTurn = mta.ExecuteTurn();
        yield return null;

        TestTurn.UpdateAllCultures();

        Assert.That(TestCulture.currentState == Culture.State.Moving, "Culture is not moving!");

        yield return new WaitForSeconds(.03f);

        Assert.That(TestCultureObj.transform.position != TestTile.transform.position);

        yield return new WaitForSeconds(.1f);

        Turn.HookTurn().UpdateAllCultures();



        Assert.That(TestCulture.currentState == Culture.State.NewOnTile, "Culture has not returned to default state!");
        Assert.That(TestCulture.transform.parent == NeighborTile.transform, "Culture has not changed tiles!");
        Assert.That(!TestTile.GetComponent<TileInfo>().cultures.ContainsKey(TestCulture.name), "Previous culture is still in old tileinfo!");
    }

    [UnityTest]
    public IEnumerator MoveTileActionWhenLargeTest()
    {
        Turn.HookTurn().UpdateCulture(TestCulture).popChange = 20;
        Turn.HookTurn().UpdateAllCultures();

        Assert.That(TestCulture.Population == 21, "test culture's population is not 6!");


        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.moveChance = 1;
        Turn testTurn = mta.ExecuteTurn();

        yield return null;

        Turn.HookTurn().UpdateAllCultures();

        Assert.That(TestCulture.Population < TestCulture.maxPopTransfer && TestCulture.Population > TestCulture.minPopTransfer, $"Expected pop to be in range {TestCulture.minPopTransfer}, {TestCulture.maxPopTransfer} but pop is actually {TestCulture.Population}!");

        yield return new WaitForSeconds(.1f);

        Turn.HookTurn().UpdateAllCultures();

        GameObject childCultureObj = NeighborTile.transform.GetChild(1).gameObject;

        Assert.That(childCultureObj != null, "child culture is not child of new tile!");

        Culture childCulture = childCultureObj.GetComponent<Culture>();

        Assert.That(childCulture != null, "Tile child is not a culture object!");

        Assert.That(childCulture.Population < childCulture.maxPopTransfer && childCulture.Population > childCulture.minPopTransfer, $"Expected pop to be in range {childCulture.minPopTransfer}, {childCulture.maxPopTransfer} but pop is actually {childCulture.Population}!");
        Assert.That(childCulture.currentState == Culture.State.NewOnTile, "Child's state is not NewOnTile!");



    }


    [UnityTest]
    public IEnumerator RepelledActionTest()
    {
        TestCulture.GetComponent<CultureMemory>().previousTile = NeighborTile.GetComponent<Tile>();

        RepelledAction tra = new RepelledAction(TestCulture);
        Turn testTurn = tra.ExecuteTurn();
        testTurn.UpdateAllCultures();

        yield return null;

        Assert.That(TestCulture.currentState == Culture.State.Moving, "Culture did not change to moving!");

        yield return new WaitForSeconds(.1f);

        Turn.HookTurn().UpdateAllCultures();

        Assert.That(TestCulture.currentState == Culture.State.NewOnTile, "Culture is not in NewOnTile State!");
    }
}