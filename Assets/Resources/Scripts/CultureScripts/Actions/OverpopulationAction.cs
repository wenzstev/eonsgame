using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OverpopulationAction 
{
    static float popLossChance = .1f;
    static int numPopLost = -1;

    public static void ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        if (Random.value < popLossChance)
        {
            Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, cultureTurnInfo.Culture, numPopLost));
            return;
        }
        MovePreferredTileAction.MoveToPreferredTile(cultureTurnInfo);
    }
}
