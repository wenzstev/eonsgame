﻿using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public abstract class CultureActionTest : BasicBoardTest
{
    protected GameObject TestCultureObj;
    protected Culture TestCulture;
    protected GameObject TestTileObj;
    protected Tile TestTile;
    protected GameObject NeighborTileObj;
    protected Tile NeighborTile;

    [UnitySetUp]
    public IEnumerator SetupCultureActionTest()
    {

        TestCulture = CreateBarebonesCulture("Test Culture");
        TestCultureObj = TestCulture.gameObject;

        TestTileObj = TestBoard.GetTile(0, 0);
        TestTile = TestTileObj.GetComponent<Tile>();

        NeighborTileObj = TestBoard.GetTile(1, 0);
        NeighborTile = NeighborTileObj.GetComponent<Tile>();

        TestCulture.Init(TestTile, 1);
        Debug.Log($"Setup Complete. Culture name is {TestCulture}");
        yield return null;
    }

    public Culture CreateBarebonesCulture(string name)
    {
        GameObject CultureObj = new GameObject(name);
        CultureObj.AddComponent<SpriteRenderer>();

        Culture Culture = CultureObj.AddComponent<Culture>();

        CultureObj.AddComponent<AffinityManager>();
        CultureObj.AddComponent<CultureFoodStore>();


        CultureMemory TestCultureMemory = CultureObj.AddComponent<CultureMemory>();
        return Culture;
    }


    [TearDown]
    public void TearDownCultureTest()
    {
        Turn.UpdateAllCultures();
    }
}
