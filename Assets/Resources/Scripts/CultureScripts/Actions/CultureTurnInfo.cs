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

public static class DoNothingAction
{
    public static void DoNothing(CultureTurnInfo cultureTurnInfo){}
}

