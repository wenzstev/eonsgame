using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class OverpopulationActionTestSuite 
{
    Tile testTile;
    Culture testCultureA;
    //Culture testCultureB;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager"));
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/Controllers"));
        
        // set up board
        GameObject testBoardObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Board"));
        GameObject testBoardGeneratorObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/BoardGenerator"));

        Board testBoard = testBoardObj.GetComponent<Board>();
        testBoard.GetComponent<BoardInputReader>().bg = testBoardGeneratorObj.GetComponent<BoardGenAlgorithm>();
        BoardStats boardStats = testBoard.GetComponent<BoardStats>();

        boardStats.height = 3;
        boardStats.width = 3;

        yield return null; // CreateBoard() is run in Board's Start() command

        GameObject testTileObj = testBoard.GetTile(1, 1);
        GameObject testCultureAObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        //GameObject testCultureBObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));

        testTile = testTileObj.GetComponent<Tile>();
        testCultureA = testCultureAObj.GetComponent<Culture>();

        testCultureA.Init(testTile);
        Turn.HookTurn().UpdateCulture(testCultureA).popChange = 10;
        Turn.HookTurn().UpdateAllCultures();
    }

    [Test]
    public void CanBecomeOverPopulated()
    {
        DefaultAction da = new DefaultAction(testCultureA);
        Turn turn = da.ExecuteTurn();
        CultureTurnUpdate cta = turn.turnUpdates[testCultureA];
        Assert.That(cta.newState == Culture.State.Overpopulated, "Culture not in overpopulated state!");
    }

    [Test]
    public void TestPopulationDrop()
    {
        OverpopulationAction opa = new OverpopulationAction(testCultureA);
        opa.popLossChance = 1f;
        Turn turn = opa.ExecuteTurn();

        CultureTurnUpdate cta = turn.turnUpdates[testCultureA];
        Assert.That(cta.popChange == -1, "Culture did not lose size!");


    }

    [Test]
    public void TestPopulationMoveAndDrop()
    {
        // if population is too high,
        // culture can attempt to flee and split off
        // if that fails, some population is killed



        OverpopulationAction opa = new OverpopulationAction(testCultureA);
        opa.popLossChance = 0f;
        Turn turn = opa.ExecuteTurn();

        CultureTurnUpdate cta = turn.turnUpdates[testCultureA];
        Assert.That(turn.turnUpdates.Count == 2, "Culture did not split!");
        Assert.That(testCultureA.population == 10, "Culture did not lose size!");
    }
    


    [TearDown]
    public void TearDown()
    {
        Turn.HookTurn().UpdateAllCultures();

        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }
}
