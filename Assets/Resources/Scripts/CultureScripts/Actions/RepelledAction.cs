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

    protected override GameObject GetTargetTile()
    {
        return Culture.GetComponent<CultureMemory>().previousTile.gameObject;
    }

    Turn ReturnToPreviousTile()
    {
        //Debug.Log(culture.GetComponent<CultureMemory>().previousTile);

        return ExecuteMove();
    }

    Turn WasPreviouslyRepelled()
    {
        MoveRandomTileAction mta = new MoveRandomTileAction(Culture);
        return mta.ExecuteTurn();
    }
 
}
