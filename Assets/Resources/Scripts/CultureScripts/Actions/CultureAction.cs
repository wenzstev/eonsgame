using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureAction
{
    public Culture Culture { get; private set; }
    public Turn turn;

    protected float ActionCost = 1; // cost per pop for this culture to perform this action

    protected CultureAction(Culture c)
    {
        //Debug.Log("starting turn for " + c.GetHashCode());
        Culture = c;
        turn = Turn.CurrentTurn;
        //Debug.Log($"{culture.Population}, {ActionCost}");
        Turn.AddUpdate(new FoodUpdate(this, Culture, -ActionCost * Culture.Population)); // doing it here means you lose additional food per turn if you call more than one action per turn
        //Debug.Log(turn.UpdateCulture(culture).FoodChange);
    }

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

