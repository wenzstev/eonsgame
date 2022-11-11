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
        Turn TestTurn = SetFoodAndExecuteTurn();

        Assert.That(TestTurn.turnUpdates[TestCulture].FoodChange == 10 - 1, $"FoodChange is {TestTurn.turnUpdates[TestCulture].FoodChange} but should be 9!"); // 10 is 1 percent of 1000, minus 1 cost for the action

        yield return null;
    }

    [UnityTest]
    public IEnumerator CanConsumeFoodInTurn()
    {
        TestCultureFoodStore.CurrentFoodStore = 1000;
        DefaultAction TestDefaultAction = new DefaultAction(TestCulture);
        Turn turn = TestDefaultAction.ExecuteTurn();
        Assert.That(turn.turnUpdates[TestCulture].FoodChange == -1, $"Culture should be reducing food but FoodChange is {turn.turnUpdates[TestCulture].FoodChange}!");
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanConsumeFoodMultiplePopulation()
    {
        Turn.HookTurn().UpdateCulture(TestCulture).popChange = 5;
        Turn.HookTurn().UpdateCulture(TestCulture).FoodChange = 1000;
        Turn.HookTurn().UpdateAllCultures();

        
        DefaultAction TestDefaultAction = new DefaultAction(TestCulture);
        Turn turn = TestDefaultAction.ExecuteTurn();
        Assert.That(turn.turnUpdates[TestCulture].FoodChange == -6, $"Culture should be reducing food but FoodChange is {turn.turnUpdates[TestCulture].FoodChange}!");
        yield return null;
    }

    [UnityTest]
    public IEnumerator CanReduceFoodOnTile()
    {
        Turn TestTurn = SetFoodAndExecuteTurn();
        Assert.That(TestTile.GetComponent<TileFood>().CurFood == 990, $"Food on tile should have been reduced to 990 but is instead {TestTile.GetComponent<TileFood>().CurFood}!");
        yield return null;
    }

    Turn SetFoodAndExecuteTurn()
    {
        TestTile.GetComponent<TileFood>().CurFood = 1000; // set high to make sure that the culture gathers enough
        GatherFoodAction TestGatherFoodAction = new GatherFoodAction(TestCulture);
        return TestGatherFoodAction.ExecuteTurn();
    }
}