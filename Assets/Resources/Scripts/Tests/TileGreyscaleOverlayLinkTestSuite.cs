using UnityEditor;
using UnityEngine;
using NUnit.Framework;

public class TileGreyscaleOverlayLinkTestSuite
{
    GameObject TestTileObject;
    TileFood TestTileFood;
    TileGreyscaleOverlayLink TestTileGreyscaleOverlayLink;
    GreyscaleOverlay TestGreyscaleOverlay;

    [SetUp]
    public void OnSetUp()
    {
        // link everything up
        TestTileObject = new GameObject();
        TestTileObject.AddComponent<TileChars>();
        TestTileFood = TestTileObject.AddComponent<TileFood>();
        TestTileObject.AddComponent<SpriteRenderer>();

        GameObject TileGreyscaleOverlayObject = new GameObject();
        TileGreyscaleOverlayObject.AddComponent<SpriteRenderer>();
        TileGreyscaleOverlayObject.transform.SetParent(TestTileObject.transform);

        TestTileGreyscaleOverlayLink = TileGreyscaleOverlayObject.AddComponent<TileGreyscaleOverlayLink>();
        TestTileGreyscaleOverlayLink.targetSprite = TestTileObject.GetComponent<SpriteRenderer>();
        TestGreyscaleOverlay = TileGreyscaleOverlayObject.AddComponent<GreyscaleOverlay>();
        
        TestTileGreyscaleOverlayLink.Initialize();
    }


    [Test]
    public void CanSetToGreyIfLow()
    {
        TestTileFood.NewFoodPerTick = 1;
        TestTileFood.MAX_FOOD_MODIFIER = 1000f;

        (float, float)[] testValues = new (float, float)[]
        {
            (100, .5f), (200, 0f), (50, .75f), (500, 0f)
        };

        foreach((float, float) testValue in testValues)
        {
            TestTileFood.CurFood = testValue.Item1;
            TestTileFood.FireFoodAction();
            Assert.AreEqual(testValue.Item2, TestGreyscaleOverlay.PercentTransparent, $"Test Food amount: {testValue.Item1} -- Greyscale Overlay is not expected transparency!");
        }
    }

    [TearDown]
    public void OnTearDown()
    {
        TestUtils.TearDownTest();
    }

}
