using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

public class TileFoodTestSuite : BasicBoardTest
{

    GameObject TestTileObj;
    Tile TestTile;
    TileChars TestTileChars;
    TileFood TestTileFood;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        TestTileObj = TestBoard.GetTile(0, 0);
        TestTile = TestTileObj.GetComponent<Tile>();
        TestTileChars = TestTileObj.GetComponent<TileChars>();
        TestTileFood = TestTileObj.GetComponent<TileFood>();


        yield return null;
    }

    [Test]
    public void CanCalculateProperFoodRateDesert()
    {
        TestTileChars.precipitation = 25f; // expect contribution of .046f
        TestTileChars.temperature = 24f; // expect contribution of .4f

        TestTileFood.CalculateFoodPerTick();
        Assert.AreEqual(.446f, TestUtils.ThreeDecimals(TestTileFood.NewFoodPerTick), "Did not calculate the right new food!");
    }

    [Test]
    public void CanCalculateProperFoodRateTundra()
    {
        TestTileChars.precipitation = 40f; // expect contribution of .083f
        TestTileChars.temperature = 5f; // expect contribution of -.018f
        TestTileFood.CalculateFoodPerTick();

        Assert.AreEqual(0.065f, TestUtils.ThreeDecimals(TestTileFood.NewFoodPerTick), "Did not calculate right new food!");
    }

    [Test]
    public void CanCalculateProperFoodRateRainforest()
    {
        TestTileChars.precipitation = 318.3406f; // expect contribution of 1f
        TestTileChars.temperature = 24.58829f; // expect contribution of .399f
        TestTileFood.CalculateFoodPerTick();

        Assert.AreEqual(1.399f, TestUtils.ThreeDecimals(TestTileFood.NewFoodPerTick), "Did not calculate right new food!");


    }

    [UnityTest]
    public IEnumerator CanIncreaseFoodPerTick()
    {
        TestTileFood.NewFoodPerTick = 1.4f;
        TestTileFood.SetFood(0);

        EventManager.TriggerEvent("Tick", null);
        yield return null;

        Assert.AreEqual(1.4f, TestTileFood.CurFood, "Food did not increase by expected amount!");
    }

    [UnityTest]
    public IEnumerator CanSetFoodToMaxAtStart()
    {
        TestTileChars.precipitation = 40f; // expect contribution of .083f
        TestTileChars.temperature = 5f; // expect contribution of -.018f
        TestTileFood.CalculateFoodPerTick();
        TestTileFood.SetMaxFood();
        yield return null;
        Assert.AreEqual(Mathf.FloorToInt(TestTileFood.NewFoodPerTick * 1000f), TestTileFood.CurFood, "Curfood was set to the wrong amount!");
    }

    [UnityTest]
    public IEnumerator CanPreventAdditionalFood()
    {
        TestTileChars.precipitation = 40f; // expect contribution of .083f
        TestTileChars.temperature = 5f; // expect contribution of -.018f
        TestTileFood.CalculateFoodPerTick();
        TestTileFood.SetMaxFood();
        yield return null;
        EventManager.TriggerEvent("Tick", null);
        Assert.That(TestTileFood.MaxFood >= TestTileFood.CurFood, $"Max was {TestTileFood.MaxFood} but CurFood was {TestTileFood.CurFood}");


    }
}