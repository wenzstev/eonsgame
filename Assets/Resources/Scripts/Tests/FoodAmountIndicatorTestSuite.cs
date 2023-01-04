using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using NUnit.Framework;

public class FoodAmountIndicatorTestSuite : BasicTest
{
    FoodAmountIndicatorGenerator TestFoodAmountIndicatorGenerator;
    CultureFoodStore TestCultureFoodStore;

    [UnitySetUp]
    public IEnumerator SetUpFoodIndicatorTest()
    {
        GameObject TimeControllerObj = new GameObject("Time Controller");
        TimeControllerObj.AddComponent<TimeController>();
        TimeController.instance.speeds = new float[] { 1 };

        GameObject TestObject = new GameObject("TestStoreObj");
        TestCultureFoodStore = TestObject.AddComponent<CultureFoodStore>();
        TestFoodAmountIndicatorGenerator = TestObject.AddComponent<FoodAmountIndicatorGenerator>();
        TestFoodAmountIndicatorGenerator.TicksBetweenIndicators = new int[] { 1, 7, 60, -1 };
        TestFoodAmountIndicatorGenerator.FoodStore = TestCultureFoodStore;
        TestFoodAmountIndicatorGenerator.FoodAmountIndicatorTemplate = Resources.Load<GameObject>("Prefabs/Board/Inhabitants/FoodAmountIndicator");
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanGenerateFoodIndicatorAtExpectedInterval()
    {
        yield return SetFoodAndPassTime(7);

        Assert.AreEqual(1, TestFoodAmountIndicatorGenerator.transform.childCount, "FoodAmountIndicatorGenerator did not create a FoodAmountIndicator!");
    }

    [UnityTest]
    public IEnumerator CanDisplayAccurateFoodCount()
    {
        yield return SetFoodAndPassTime(3);
        GameObject TestFoodAmountIndicator = TestFoodAmountIndicatorGenerator.transform.GetChild(0).gameObject;

        Assert.AreEqual(30f, TestFoodAmountIndicator.GetComponent<FoodAmountIndicator>().Amount, "Indicator is not displaying the correct food!");       
    }

    IEnumerator SetFoodAndPassTime(int ticksBetween)
    {
        TestFoodAmountIndicatorGenerator.CurTicksBetweenIndicators = ticksBetween;

        foreach (var _ in Enumerable.Range(0, ticksBetween)) 
        {
            TestCultureFoodStore.AlterFoodStore(10);
            TestFoodAmountIndicatorGenerator.TickExecuted();
            yield return null;
        }
    }

}