using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelledAction : CultureMoveAction
{
    public RepelledAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return ReturnToPreviousTile();
    }

    Turn ReturnToPreviousTile()
    {
        culture.StartCoroutine(MoveTile(culture.gameObject, culture.GetComponent<CultureMemory>().previousTile.gameObject));
        turn.UpdateCulture(culture).newState = Culture.State.Moving;
        return turn;
    }
 
}
