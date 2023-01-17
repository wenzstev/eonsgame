using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICultureAction
{
    public Culture Culture { get; }
    float ActionCost { get; }


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

public class DoNothingAction : CultureAction
{
    public DoNothingAction(Culture c) : base(c) { }
    public override Turn ExecuteTurn()
    {
        return turn;
    }
}

