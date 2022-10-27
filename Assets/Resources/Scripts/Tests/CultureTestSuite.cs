using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class CultureTestSuite 
{
    Culture testCulture;
    Tile testTile;

    [SetUp]
    public void SetUp()
    {
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Controllers/EventManager"));
        GameObject testCultureObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        testCulture = testCultureObj.GetComponent<Culture>();

        GameObject testTileObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board/Tile"));
        testTile = testTileObj.GetComponent<Tile>();

        testCulture.Init(testTile);
    }

    [Test]
    public void TestSplitCulture()
    {
        
        testCulture.AddPopulation(4);
        int oldPopulation = testCulture.Population;

        GameObject splitCultureObj = testCulture.SplitCultureFromParent();
        Culture splitCulture = splitCultureObj.GetComponent<Culture>();

        Assert.That(splitCulture.name == testCulture.name, "Split culture's name doesn't match!");
        Assert.That(splitCulture.Population == testCulture.maxPopTransfer, "Split culture's size doesn't match split amount!");
        Assert.That(testCulture.Population == oldPopulation - testCulture.maxPopTransfer, "Parent culture's population wasn't lowered!");
        Assert.That(splitCulture.GetComponent<CultureMemory>().cultureParentName == testCulture.GetComponent<CultureMemory>().cultureParentName, "Split culture doesn't have the same parent!");
        Assert.That(testCulture.Tile == splitCulture.transform.parent.GetComponent<Tile>(), "Split culture wasn't set as parent of tile!");
    }

    [Test]
    public void TestCanMerge()
    {
        GameObject mergeCultureObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CultureLayer"));
        Culture mergeCulture = mergeCultureObj.GetComponent<Culture>();

        mergeCulture.SetColor(Color.blue);

        Assert.That(!testCulture.CanMerge(mergeCulture), "CanMerge returned true when should return false!");

        testCulture.SetColor(Color.blue);

        Assert.That(testCulture.CanMerge(mergeCulture), "CanMerge returned false when should return true!");
        
        
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
