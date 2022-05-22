using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelledAction : CultureMoveAction
{
    public RepelledAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        turn.newState = ReturnToPreviousTile();
        return turn;
    }

    Culture.State ReturnToPreviousTile()
    {
        culture.StartCoroutine(MoveTile(culture.gameObject, culture.GetComponent<CultureMemory>().previousTile.gameObject));
        return Culture.State.Moving;
    }
 
}
