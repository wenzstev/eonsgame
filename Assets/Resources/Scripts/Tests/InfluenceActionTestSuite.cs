using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class InfluenceActionTestSuite 
{
    Culture testCulture;
    Culture neighbor;
    Tile testTile;

    [SetUp]
    public void Setup()
    {
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager"));
        GameObject testTileObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Tile"));
        testTile = testTileObj.GetComponent<Tile>();

        GameObject testCultureObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        testCulture = testCultureObj.GetComponent<Culture>();

        GameObject neighborObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        neighbor = neighborObj.GetComponent<Culture>();

        testCulture.Init(testTile);
        neighbor.Init(testTile);
    }

    [Test]
    public void CanInfluenceNeighborCulture()
    {
        CultureInfluenceAction cia = new CultureInfluenceAction(testCulture);
        Turn turn = cia.ExecuteTurn();

        Assert.That(turn.turnUpdates.ContainsKey(neighbor), "Neighbor experiencing no changes!");

        CultureTurnUpdate neighborUpdate = turn.turnUpdates[neighbor];

        Assert.That(CultureHelperMethods.GetColorDistance(testCulture.color, neighborUpdate.newColor) 
                    < CultureHelperMethods.GetColorDistance(testCulture.color, neighbor.color),
                    "Cultures are not closer together!");

        
    }

    [Test]
    public void CanMergeIntoNewCultureIfClose()
    {
        Turn.HookTurn().UpdateCulture(testCulture).newColor = Color.blue;
        Turn.HookTurn().UpdateCulture(neighbor).newColor = Color.blue;
        Turn.HookTurn().UpdateAllCultures();

        CultureInfluenceAction cia = new CultureInfluenceAction(testCulture);
        Turn turn = cia.ExecuteTurn();

        Assert.That(turn.turnUpdates.ContainsKey(testCulture), "Test culture experiencing no changes!");
        Assert.That(turn.turnUpdates.ContainsKey(neighbor), "Neighbor experiencing no changes!");

        CultureTurnUpdate testCultureUpdate = turn.turnUpdates[testCulture];
        CultureTurnUpdate neighborUpdate = turn.turnUpdates[neighbor];

        Assert.That(neighborUpdate.newState == Culture.State.PendingRemoval, "Neighbor not slated for removal!");
        Assert.That(testCultureUpdate.newName != "", "Culture not getting new name!");
        Assert.That(testCultureUpdate.popChange == neighbor.population, "Culture not gaining members of neighbor culture!");
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
