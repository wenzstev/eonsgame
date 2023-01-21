using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureTurn
{
    public Culture Culture { get; }
    float ActionCost { get; }

    Turn Turn { get; set; }


    /// <summary>
    /// Get the cost of this action. Returns a negative value.
    /// </summary>
    /// <returns></returns>
    protected float GetActionCost()
    {
        return -ActionCost * Culture.Population;
    }

    public abstract Turn ExecuteTurn();

}

<<<<<<< HEAD
public static class DoNothingAction
{
    public static void DoNothing(CultureTurnInfo cultureTurnInfo){}
=======
public static class DoNothingAction : CultureTurnInfo
{
    

    public Turn ExecuteTurn()
    {
        return turn;
    }
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
}

