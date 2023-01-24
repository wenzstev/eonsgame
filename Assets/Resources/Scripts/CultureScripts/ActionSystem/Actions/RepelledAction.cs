using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RepelledAction
{

    public static void RepelCulture(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        if(culture.CultureMemory.wasRepelled || culture.CultureMemory.previousTile == null)
        {
            WasPreviouslyRepelled(cultureTurnInfo);
            return;
        }

        ReturnToPreviousTile(cultureTurnInfo);
    }

    static Tile GetTargetTile(Culture c)
    {
        return c.CultureMemory.previousTile;
    }

    static void ReturnToPreviousTile(CultureTurnInfo cultureTurnInfo)
    {
        //Debug.Log(culture.GetComponent<CultureMemory>().previousTile);

        Turn.AddTileUpdate(CultureUpdateGetter.GetMoveUpdate(cultureTurnInfo, cultureTurnInfo.Culture, GetTargetTile(cultureTurnInfo.Culture)));
    }

    static void WasPreviouslyRepelled(CultureTurnInfo cultureTurnInfo)
    {
        MoveRandomTileAction.MoveRandomTile(cultureTurnInfo);
    }
 
}
