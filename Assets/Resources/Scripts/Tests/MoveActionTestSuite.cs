using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class MoveActionTestSuite 
{
    GameObject testCultureObj;
    GameObject homeTile;
    GameObject projectedTile;
    Culture testCulture;



    [UnitySetUp]
    public IEnumerator SetUp()
    {
        Object.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager"));
        Object.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/Controllers"));

        GameObject testBoardObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Board"));
        GameObject testBoardGeneratorObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/BoardGenerator"));
        
        Board testBoard = testBoardObj.GetComponent<Board>();
        testBoard.GetComponent<BoardInputReader>().bg = testBoardGeneratorObj.GetComponent<BoardGenerator>();

        testBoard.height = 1;
        testBoard.width = 2;

        yield return null;

        homeTile = testBoard.tiles.GetTile(0, 0);
        projectedTile = testBoard.tiles.GetTile(1, 0);

        testCultureObj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        testCulture = testCultureObj.GetComponent<Culture>();

        testCulture.Init(homeTile.GetComponent<Tile>());

        yield return null;


    }


    [UnityTest]
    public IEnumerator MoveTileActionTest()
    {
        MoveTileAction mta = new MoveTileAction(testCulture);
        mta.moveChance = 1;

        Turn testTurn = mta.ExecuteTurn();

        int counter = 0;
        while(testTurn.turnUpdates[testCulture].newState == Culture.State.Default && counter < 50)
        {
            mta = new MoveTileAction(testCulture);
            mta.moveChance = 1;
            testTurn = mta.ExecuteTurn();
            counter++;
        }

        yield return null;

        testTurn.UpdateAllCultures();


        Assert.That(counter < 50, "Test ran too many times without ever moving the tile!");
        Assert.That(testCulture.currentState == Culture.State.Moving, "Culture is not moving!");


        yield return new WaitForSeconds(.03f);

        Assert.That(testCultureObj.transform.position != homeTile.transform.position);

        yield return new WaitForSeconds(.1f);

        Turn.HookTurn().UpdateAllCultures();

      

        Assert.That(testCulture.currentState == Culture.State.NewOnTile, "Culture has not returned to default state!");
        Assert.That(testCulture.transform.parent == projectedTile.transform, "Culture has not changed tiles!");
        Assert.That(!homeTile.GetComponent<TileInfo>().cultures.ContainsKey(testCulture.name), "Previous culture is still in old tileinfo!");   
    }

    [UnityTest]
    public IEnumerator MoveTileActionWhenLargeTest()
    {
        Turn.HookTurn().UpdateCulture(testCulture).popChange = 5;
        Turn.HookTurn().UpdateAllCultures();

        Assert.That(testCulture.population == 6, "test culture's population is not 6!");

        MoveTileAction mta = new MoveTileAction(testCulture);
        mta.moveChance = 1;

        Turn testTurn = mta.ExecuteTurn();

        int counter = 0;
        while (testTurn.turnUpdates.Count == 1 && counter < 50)
        {
            Debug.Log("Counter at " + counter);
            mta = new MoveTileAction(testCulture);
            mta.moveChance = 1;
            testTurn = mta.ExecuteTurn();
            counter++;
        }

        yield return null;

        Turn.HookTurn().UpdateAllCultures();

        Assert.That(counter < 50, "Test ran too many times without ever moving the tile!");
        Assert.That(testCulture.population == 5, "testCulture did not lose population!");

        yield return new WaitForSeconds(.1f);

        Turn.HookTurn().UpdateAllCultures();

        GameObject childCultureObj = projectedTile.transform.GetChild(1).gameObject;

        Assert.That(childCultureObj != null, "child culture is not child of new tile!");

        Culture childCulture = childCultureObj.GetComponent<Culture>();

        Assert.That(childCulture != null, "Tile child is not a culture object!");

        Assert.That(childCulture.population == 1, "Child culture's population is not 1!");
        Assert.That(childCulture.currentState == Culture.State.NewOnTile, "Child's state is not NewOnTile!");



    }


    [UnityTest]
    public IEnumerator RepelledActionTest()
    {
        testCulture.GetComponent<CultureMemory>().previousTile = projectedTile.GetComponent<Tile>();

        RepelledAction tra = new RepelledAction(testCulture);
        Turn testTurn = tra.ExecuteTurn();
        testTurn.UpdateAllCultures();

        yield return null;

        Assert.That(testCulture.currentState == Culture.State.Moving, "Culture did not change to moving!");

        yield return new WaitForSeconds(.1f);

        Turn.HookTurn().UpdateAllCultures();

        Assert.That(testCulture.currentState == Culture.State.NewOnTile, "Culture is not in NewOnTile State!");



    }

    [TearDown]
    public void TearDown()
    {
        Turn.HookTurn().UpdateAllCultures();

        foreach(GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }


}
