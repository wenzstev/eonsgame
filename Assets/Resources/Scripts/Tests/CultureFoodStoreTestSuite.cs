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
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanCollectFoodFromTile()
    {
        float PreTickFood = TestCultureFoodStore.CurrentFoodStore;
        TestTile.GetComponent<TileFood>().CurFood = 1000; // set high to make sure that the culture gathers enough
        GatherFoodAction TestGatherFoodAction = new GatherFoodAction(TestCulture);
        Turn turn = TestGatherFoodAction.ExecuteTurn();

        Assert.That(turn.turnUpdates[TestCulture].FoodChange == 10 - 1, $"FoodChange is {turn.turnUpdates[TestCulture].FoodChange} but should be 9!"); // 10 is 1 percent of 1000, minus 1 cost for the action

        yield return null;
    }
}