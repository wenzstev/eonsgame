using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureAction
{
    protected Culture culture;
    public Turn turn;

    protected float ActionCost = 1; // cost per pop for this culture to perform this action

    protected CultureAction(Culture c)
    {
        //Debug.Log("starting turn for " + c.GetHashCode());
        culture = c;
        turn = Turn.HookTurn();
        //Debug.Log($"{culture.Population}, {ActionCost}");
        turn.UpdateCulture(culture).FoodChange = -ActionCost * culture.Population;
        //Debug.Log(turn.UpdateCulture(culture).FoodChange);
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

