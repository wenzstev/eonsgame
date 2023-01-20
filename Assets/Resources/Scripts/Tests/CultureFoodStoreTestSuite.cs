using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;

public class CultureFoodStoreTestSuite : CultureActionTest 
{
    CultureFoodStore TestCultureFoodStore;

    [UnitySetUp]
    public IEnumerator GetCultureFood()
    {
        TestCultureFoodStore = TestCultureObj.AddComponent<CultureFoodStore>();
        TestCultureFoodStore.StorePerPopulation = 100;
        MonoBehaviour.Destroy(TestCulture.GetComponent<AffinityManager>()); // remove the affinitymanager to prevent cross-pollination of tests 
        TestCulture.FoodGatherRate = .01f;

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanCollectFoodFromTile()
    {
        SetFoodAndExecuteTurn();
        AssertFoodChange(10 - 2);
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanConsumeFoodInTurn()
    {
        TestTile.GetComponent<TileFood>().CurFood = 0;
        TestCultureFoodStore.AlterFoodStore(TestCultureFoodStore.MaxFoodStore);
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        DefaultAction.ExecuteTurn(cultureTurnInfo);

        AssertFoodChange(-2);
        yield return null;
    }


    [UnityTest]
    public IEnumerator CanConsumeFoodMultiplePopulation()
    {
        TestTile.GetComponent<TileFood>().CurFood = 0;
        TestCulture.AddPopulation(5);
        TestCulture.GetComponent<CultureFoodStore>().AlterFoodStore(1000);


        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        DefaultAction.ExecuteTurn(cultureTurnInfo);

        AssertFoodChange(-12);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanReduceFoodOnTile()
    {
        SetFoodAndExecuteTurn();
        Assert.AreEqual(991, TestTile.GetComponent<TileFood>().CurFood, "Food on tile is not what's expected!");
        yield return null;
    }

    void SetFoodAndExecuteTurn()
    {
        TestTile.GetComponent<TileFood>().CurFood = 1000;
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        GatherFoodAction.GatherFood(cultureTurnInfo);
    }

    void AssertFoodChange(float expected)
    {
        INonGenericCultureUpdate[] TestCultureUpdateList = Turn.GetPendingUpdatesFor(TestCulture);
        Assert.AreEqual(expected, TestUtils.GetCombinedFoodChangeInUpdateList(TestCultureUpdateList), "FoodChange is incorrect!");
    }

}