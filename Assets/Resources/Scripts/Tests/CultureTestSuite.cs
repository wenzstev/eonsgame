using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class CultureTestSuite 
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

        testCultureObj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"), Vector3.zero, Quaternion.identity);
        testCulture = testCultureObj.GetComponent<Culture>();

        homeTile = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Tile"), Vector3.zero, Quaternion.identity);
        projectedTile = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Tile"), Vector3.right, Quaternion.identity);

        yield return new WaitForFixedUpdate();

        testCulture.Init(homeTile.GetComponent<Tile>());


        testCultureObj.transform.position = homeTile.transform.position;
        testCultureObj.transform.SetParent(homeTile.transform);
        homeTile.GetComponent<TileInfo>().AddCulture(testCulture);


    }
    

    [UnityTest]
    public IEnumerator TestMoveTile()
    {
        //testCulture.StartCoroutine(CultureMoveAction.MoveTile(testCulture.gameObject, projectedTile));

        Assert.That(projectedTile.GetComponent<TileInfo>().hasTransition == true, "Projected tile doesn't have its animation set!");


        yield return null;

        Assert.That(projectedTile.GetComponent<TileInfo>().hasTransition == true, "Projected tile doesn't have its animation set!");

        yield return new WaitForSeconds(.05f);

        Assert.That(projectedTile.GetComponent<TileInfo>().hasTransition == true, "Projected tile doesn't have its animation set!");

        yield return new WaitForSeconds(.06f);

        Assert.That(testCulture.tile == projectedTile.GetComponent<Tile>(), "Culture doesn't have new tile set as it's tile!");
        Assert.That(projectedTile.GetComponent<TileInfo>().hasTransition == false, "Tile says transition is ongoing!");
        Assert.That(testCulture.transform.parent == projectedTile.transform, "Culture's parent isn't the new tile!");
        Assert.That(testCulture.transform.position == projectedTile.transform.position, "Culture is not directly on tile!");
        Assert.That(projectedTile.GetComponent<TileInfo>().cultures.ContainsKey(testCulture.name), "New tile isn't set to have this culture!");
        Assert.That(!homeTile.GetComponent<TileInfo>().cultures.ContainsKey(testCulture.name), "Old tile is still referencing culture!");
    }


    [TearDown]
    public void TearDown()
    {
        foreach(GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }


}
