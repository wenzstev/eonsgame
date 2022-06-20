using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverpopulationAction : CultureAction
{
    public float popLossChance = .01f;
    public OverpopulationAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        if (Random.value < popLossChance)
        {
            turn.UpdateCulture(culture).popChange -= 1;
            return turn;
        }

        MoveTileAction moveTile = new MoveTileAction(culture);
        moveTile.moveChance = 1;
        return moveTile.ExecuteTurn();
    }
}
