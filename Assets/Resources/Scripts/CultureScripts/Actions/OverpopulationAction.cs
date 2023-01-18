using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverpopulationAction : CultureTurnInfo
{
    public float popLossChance = .1f;
    public int numPopLost = -1;
    public OverpopulationAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        if (Random.value < popLossChance)
        {
            Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, Culture, numPopLost));
            return turn;
        }

        MovePreferredTileAction moveTile = new MovePreferredTileAction(Culture);
        return moveTile.ExecuteTurn();
    }
}
