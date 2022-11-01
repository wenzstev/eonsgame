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

        Turn TestTurn = TestMoveAbility();

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
        Turn.HookTurn().UpdateCulture(TestCulture).popChange = 5;
        Turn.HookTurn().UpdateAllCultures();

        Assert.That(TestCulture.Population == 6, "test culture's population is not 6!");

        
        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.moveChance = 1;

        Turn testTurn = mta.ExecuteTurn();

        int counter = 0;
        while (testTurn.turnUpdates.Count == 1 && counter < 50)
        {
            mta = new MoveTileAction(TestCulture);
            mta.moveChance = 1;
            testTurn = mta.ExecuteTurn();
            counter++;
        }
        

        yield return null;

        Turn.HookTurn().UpdateAllCultures();

        Assert.That(counter < 50, "Test ran too many times without ever moving the tile!");
        Assert.That(TestCulture.Population == 5, "TestCulture did not lose population!");

        yield return new WaitForSeconds(.1f);

        Turn.HookTurn().UpdateAllCultures();

        GameObject childCultureObj = NeighborTile.transform.GetChild(1).gameObject;

        Assert.That(childCultureObj != null, "child culture is not child of new tile!");

        Culture childCulture = childCultureObj.GetComponent<Culture>();

        Assert.That(childCulture != null, "Tile child is not a culture object!");

        Assert.That(childCulture.Population == 1, "Child culture's population is not 1!");
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

    // TODO: refactor this so that it works for more tests?
    Turn TestMoveAbility()
    {
        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.moveChance = 1;
        Turn testTurn = mta.ExecuteTurn();

        int counter = 0;
        while (testTurn.turnUpdates[TestCulture].newState == Culture.State.Default && counter < 50)
        {
            mta = new MoveTileAction(TestCulture);
            mta.moveChance = 1;
            testTurn = mta.ExecuteTurn();
            counter++;
        }

        Assert.That(counter < 50, "Test ran too many times without ever moving the tile!");
        return testTurn;
    }
}
