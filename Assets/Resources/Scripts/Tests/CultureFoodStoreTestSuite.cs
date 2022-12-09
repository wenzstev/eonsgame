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
        TestCultureFoodStore = TestCulture.GetComponent<CultureFoodStore>();
        TestCultureFoodStore.StorePerPopulation = 100;
        MonoBehaviour.Destroy(TestCulture.GetComponent<AffinityManager>()); // remove the affinitymanager to prevent cross-pollination of tests 
        TestCulture.FoodGatherRate = .01f;

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanCollectFoodFromTile()
    {
        SetFoodAndExecuteTurn();
        AssertFoodChange(10 - 1);
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanConsumeFoodInTurn()
    {
        TestTile.GetComponent<TileFood>().CurFood = 0;
        TestCultureFoodStore.AlterFoodStore(1000);
        DefaultAction TestDefaultAction = new DefaultAction(TestCulture);
        TestDefaultAction.ExecuteTurn();

        AssertFoodChange(-1);
        yield return null;
    }


    [UnityTest]
    public IEnumerator CanConsumeFoodMultiplePopulation()
    {
        TestTile.GetComponent<TileFood>().CurFood = 0;
        TestCulture.AddPopulation(5);
        TestCulture.GetComponent<CultureFoodStore>().AlterFoodStore(1000);

        
        DefaultAction TestDefaultAction = new DefaultAction(TestCulture);
        Turn turn = TestDefaultAction.ExecuteTurn();

        AssertFoodChange(-6);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanReduceFoodOnTile()
    {
        SetFoodAndExecuteTurn();
        Assert.AreEqual(990, TestTile.GetComponent<TileFood>().CurFood, "Food on tile is not what's expected!");
        yield return null;
    }

    void SetFoodAndExecuteTurn()
    {
        TestTile.GetComponent<TileFood>().CurFood = 1000; 
        GatherFoodAction TestGatherFoodAction = new GatherFoodAction(TestCulture);
        TestGatherFoodAction.ExecuteTurn();
    }

    void AssertFoodChange(float expected)
    {
        INonGenericCultureUpdate[] TestCultureUpdateList = Turn.GetPendingUpdatesFor(TestCulture);

        Assert.AreEqual(expected, TestUtils.GetCombinedFoodChangeInUpdateList(TestCultureUpdateList), "FoodChange is incorrect!");
    }

}