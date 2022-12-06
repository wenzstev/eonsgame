using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelledAction : CultureMoveAction
{
    public RepelledAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        if(Culture.GetComponent<CultureMemory>().wasRepelled)
        {
            return WasPreviouslyRepelled();
        }

        return ReturnToPreviousTile();
    }

    Turn ReturnToPreviousTile()
    {
        //Debug.Log(culture.GetComponent<CultureMemory>().previousTile);

        Culture.StartCoroutine(MoveTile(Culture.gameObject, Culture.GetComponent<CultureMemory>().previousTile.gameObject));
        turn.UpdateCulture(Culture).newState = Culture.State.Moving;
        return turn;
    }

    Turn WasPreviouslyRepelled()
    {
        MoveTileAction mta = new MoveTileAction(Culture);
        return mta.ExecuteTurn();
    }
 
}
