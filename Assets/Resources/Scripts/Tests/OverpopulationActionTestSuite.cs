using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class OverpopulationActionTestSuite 
{
    Tile testTile;
    Culture testCultureA;
    Culture testCultureB;

    [SetUp]
    public void SetUp()
    {
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager"));
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/Controllers"));

        GameObject testTileObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Tile"));
        GameObject testCultureAObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        GameObject testCultureBObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
    }

    [Test]
    public void TestPopulationDrop()
    {
        // if population is too high,
        // culture can attempt to flee and split off
        // if that fails, some population is killed
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
