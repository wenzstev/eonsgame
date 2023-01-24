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
        float amountGathered = SetFoodAndExecuteTurn();
        Assert.AreEqual(10, amountGathered);
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanConsumeFoodInTurn()
    {
        TestTile.GetComponent<TileFood>().CurFood = 0;
        TestCultureFoodStore.AlterFoodStore(TestCultureFoodStore.MaxFoodStore);
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        DefaultAction.ExecuteTurn(cultureTurnInfo);

        AssertFoodChange(-1);
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

        AssertFoodChange(-6);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanReduceFoodOnTile()
    {
        SetFoodAndExecuteTurn();
        Assert.AreEqual(991, TestTile.GetComponent<TileFood>().CurFood, "Food on tile is not what's expected!");
        yield return null;
    }

    float SetFoodAndExecuteTurn()
    {
        TestTile.GetComponent<TileFood>().CurFood = 1000;
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(TestCulture, Turn.CurrentTurn);
        return GatherFoodAction.GatherFood(cultureTurnInfo);
    }

    void AssertFoodChange(float expected)
    {
        Assert.AreEqual(expected, TestUtils.GetCombinedFoodChangeInUpdateList(Turn.CurrentTurn.UpdateHolder.GetFloatUpdates(), TestCulture), "FoodChange is incorrect!");
    }

}