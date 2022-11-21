using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using NUnit.Framework;

public class FoodAmountIndicatorTestSuite : CultureActionTest
{
    FoodAmountIndicatorGenerator TestFoodAmountIndicatorGenerator;

    [UnitySetUp]
    public IEnumerator SetUpFoodIndicatorTest()
    {
        TestFoodAmountIndicatorGenerator = TestCulture.GetComponentInChildren<FoodAmountIndicatorGenerator>();
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

        Assert.AreEqual(9f + 8.9f + 8.801f, TestFoodAmountIndicator.GetComponent<FoodAmountIndicator>().Amount, "Indicator is not displaying the correct food!");       
    }

    [UnityTest]
    public IEnumerator CanNotDuplicateIndicatorWhenSplitting()
    {
        yield return SetFoodAndPassTime(3);
        GameObject TestFoodAmountIndicator = TestFoodAmountIndicatorGenerator.transform.GetChild(0).gameObject;
        Turn.HookTurn().UpdateCulture(TestCulture).popChange = 30;
        Turn.HookTurn().UpdateAllCultures();
        NeighborTile.GetComponent<TileDrawer>().tileType = TileDrawer.BiomeType.Grassland;
        yield return null;

        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.ExecuteTurn();
        Turn.HookTurn().UpdateAllCultures();

        yield return null;

        Culture[] CultureArray = Object.FindObjectsOfType<Culture>();
        var TestCultureTest = CultureArray.Where(c => c.GetComponentInChildren<FoodAmountIndicatorGenerator>().transform.childCount == 0);
        Culture[] TestCultureOffspring = TestCultureTest.ToArray();
        Assert.AreEqual(1, TestCultureOffspring.Length, "There is not a new culture with no indicator!");
        Assert.AreEqual(1, TestCulture.GetComponentInChildren<FoodAmountIndicatorGenerator>().transform.childCount, "Original Culture has wrong number of indicators!");
    }


    IEnumerator SetFoodAndPassTime(int ticksBetween)
    {
        TestTile.GetComponent<TileFood>().CurFood = 1000;
        TestTile.GetComponent<TileFood>().NewFoodPerTick = 0;
        
        TestFoodAmountIndicatorGenerator.TicksBetweenIndicators = ticksBetween;

        foreach (var _ in Enumerable.Range(0, ticksBetween)) 
        {
            EventManager.TriggerEvent("Tick", null);
            yield return null;
        }
    }

}