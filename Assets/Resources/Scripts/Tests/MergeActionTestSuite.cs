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

        yield return null;

    }

    [UnityTest]
    public IEnumerator CanMergeCultures()
    {
        yield return null;
        Assert.That(false, "Test not yet implemented!");
    }

    [UnityTest]
    public IEnumerator CanAddNewCultureToTile()
    {
        yield return null;

        Assert.That(false, "Test not yet implemented!");
    }

    [UnityTest]
    public IEnumerator CanMergeWithParentCulture()
    {
        yield return null;

        Assert.That(false, "Test not yet implemented!");
    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }

}
