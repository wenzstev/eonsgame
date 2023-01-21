using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CultureBrain : MonoBehaviour
{
    public Culture Culture;
    public FoodAmountIndicatorGenerator FoodAmountIndicatorGenerator;

    public void Initialize()
    {
        Culture = GetComponent<Culture>();
        FoodAmountIndicatorGenerator = GetComponentInChildren<FoodAmountIndicatorGenerator>();
    }

    public void ExecuteCultureTurn()
    {
        Culture.DecisionMaker.ExecuteTurn();
<<<<<<< HEAD

        // Other components that need to know when a tick was executed

        FoodAmountIndicatorGenerator.TickExecuted();

        Turn.UpdateAllCultures();

=======
        Turn.UpdateAllCultures();

        // Other components that need to know when a tick was executed
        FoodAmountIndicatorGenerator.TickExecuted();
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }
}
