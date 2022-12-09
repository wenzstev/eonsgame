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
        TestCulture.FoodGatherRate = .01f;
        TestCulture.GetComponent<CultureFoodStore>().StorePerPopulation = 100;
        MonoBehaviour.Destroy(TestCulture.GetComponent<AffinityManager>()); // TODO: need a better way to isolate than just randomly destorying components in the setup
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
        TestCulture.AddPopulation(30);
        NeighborTile.GetComponent<TileDrawer>().tileType = TileDrawer.BiomeType.Grassland;
        yield return null;

        MoveTileAction mta = new MoveTileAction(TestCulture);
        mta.ExecuteTurn();
        Turn.UpdateAllCultures();

        yield return null;

        Culture[] CultureArray = Object.FindObjectsOfType<Culture>();
        var TestCultureTest = CultureArray.Where(c => c.GetComponentInChildren<FoodAmountIndicatorGenerator>().transform.childCount == 0);
        Culture[] TestCultureOffspring = TestCultureTest.ToArray();
        Assert.AreEqual(1, TestCultureOffspring.Length, "There is not a new culture with no indicator!");
        int numFoodIndicators = 0;
        foreach(Transform child in TestCulture.GetComponentInChildren<FoodAmountIndicatorGenerator>().transform)
        {
            if(child.GetComponent<FoodAmountIndicator>() != null) numFoodIndicators++;
        }
        Assert.AreEqual(1, numFoodIndicators, "Original Culture has wrong number of indicators!");
    }


    IEnumerator SetFoodAndPassTime(int ticksBetween)
    {
        TestTile.GetComponent<TileFood>().CurFood = 1000;
        TestTile.GetComponent<TileFood>().NewFoodPerTick = 0;

        
        TestFoodAmountIndicatorGenerator.TicksBetweenIndicators = ticksBetween;

        foreach (var _ in Enumerable.Range(0, ticksBetween)) 
        {
            EventManager.TriggerEvent("Tick", null);
            Debug.Log($"Amount is {TestCulture.GetComponent<CultureFoodStore>().CurrentFoodStore}");
            yield return null;
        }
    }

}