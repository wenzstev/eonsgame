using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureAction
{
    protected Culture culture;
    public Turn turn;

    protected CultureAction(Culture c)
    {
        //Debug.Log("starting turn for " + c.GetHashCode());
        culture = c;
        turn = Turn.HookTurn();
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

