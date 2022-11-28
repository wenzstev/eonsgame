using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class GreyscaleOverlayTestSuite
{
    GreyscaleOverlay TestGreyscaleOverlay;
    GameObject TestGreyscaleOverlayObj;
    SpriteRenderer TestSpriteRenderer;

    [SetUp]
    public void SetUpTest()
    {
        TestGreyscaleOverlayObj = new GameObject();
        TestGreyscaleOverlay = TestGreyscaleOverlayObj.AddComponent<GreyscaleOverlay>();
        TestGreyscaleOverlayObj.AddComponent<SpriteRenderer>();
        GameObject TestSpriteRendererObj = new GameObject();
        TestSpriteRenderer = TestSpriteRendererObj.AddComponent<SpriteRenderer>();

        TestSpriteRenderer.color = Color.yellow;
        TestGreyscaleOverlay.Initialize(TestSpriteRenderer);
    }

    [Test]
    public void CanMakeImageGreyscale()
    {
        Assert.AreEqual(new Color(Color.yellow.grayscale, Color.yellow.grayscale, Color.yellow.grayscale), TestGreyscaleOverlay.GreyscaleColor, "Color is not expected shade!");
    }

    [Test]
    public void CanSetToVariousTransparencies()
    {
        (float, float)[] values = new (float, float)[] {
            (1, 1),
            (.4f, .4f),
            (.7f, .7f),
            (.567f, .567f)
        };

        foreach((float, float) testValues in values)
        {
            TestGreyscaleOverlay.SetGreyscalePercentage(testValues.Item1);
            Assert.AreEqual(testValues.Item2, TestGreyscaleOverlay.PercentTransparent, "Greyscale Alpha is not at expected value!");
        }
    }

    [TearDown]
    public void TearDownTest()
    {
        TestUtils.TearDownTest();
    }

}
