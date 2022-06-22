using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelledAction : CultureMoveAction
{
    public RepelledAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        if(culture.GetComponent<CultureMemory>().wasRepelled)
        {
            return WasPreviouslyRepelled();
        }

        return ReturnToPreviousTile();
    }

    Turn ReturnToPreviousTile()
    {
        //Debug.Log(culture.GetComponent<CultureMemory>().previousTile);

        culture.StartCoroutine(MoveTile(culture.gameObject, culture.GetComponent<CultureMemory>().previousTile.gameObject));
        turn.UpdateCulture(culture).newState = Culture.State.Moving;
        return turn;
    }

    Turn WasPreviouslyRepelled()
    {
        MoveTileAction mta = new MoveTileAction(culture);
        return mta.ExecuteTurn();
    }
 
}
