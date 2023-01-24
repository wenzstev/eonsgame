using System.Collections;
using UnityEngine;

public static class StarvationAction
{

    public static float PopLossChance = .5f;
    public static int NumPopLost = 1;


    public static void ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        if(Random.value > PopLossChance)
        {
            Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, cultureTurnInfo.Culture, -NumPopLost));
        }
        MoveRandomTileAction.MoveRandomTile(cultureTurnInfo);
    }

}
