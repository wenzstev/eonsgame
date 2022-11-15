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
        Turn.HookTurn().UpdateCulture(TestCulture).popChange = 10;
        Turn.HookTurn().UpdateAllCultures();
        NeighborTile.GetComponent<TileDrawer>().tileType = TileDrawer.BiomeType.Grassland;
        yield return null;

        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.ExecuteTurn();
        Turn.HookTurn().UpdateAllCultures();

        yield return null;

        Culture TestCultureOffspring = Object.FindObjectsOfType<Culture>().Where(c => c.Population == 1).ToArray()[0];

        Assert.AreEqual(1, TestCulture.GetComponentInChildren<FoodAmountIndicatorGenerator>().transform.childCount, "Culture has wrong number of indicators!");
        Assert.AreEqual(0, TestCultureOffspring.GetComponentInChildren<FoodAmountIndicatorGenerator>().transform.childCount, "Culture offspring also has indicator!");
    }


    IEnumerator SetFoodAndPassTime(int ticksBetween)
    {
        TestTile.GetComponent<TileFood>().CurFood = 1000;
        TestTile.GetComponent<TileFood>().NewFoodPerTick = 0;
        
        TestFoodAmountIndicatorGenerator.TicksBetweenIndicators = ticksBetween;

        foreach (var _ in Enumerable.Range(0, ticksBetween)) // simulate a week
        {
            EventManager.TriggerEvent("Tick", null);
            yield return null;
        }
    }

}