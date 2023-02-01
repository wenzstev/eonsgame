using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OverpopulationAction 
{
    static float popLossChance = .1f;
    static int numPopLost = -1;
    static int starvationDays;

    public static void ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        Culture Culture = cultureTurnInfo.Culture;
        CultureMemory cultureMemory = Culture.CultureMemory;

        if (Random.value < popLossChance)
        {
            Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, Culture, numPopLost));
        }
        

        if (cultureMemory.daysSinceEaten > starvationDays) 
            Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, Culture, Culture.State.Starving));
        else
        {
            Turn.AddIntUpdate(CultureUpdateGetter.GetOverpopulationUpdate(cultureTurnInfo, Culture, cultureMemory.daysSinceEaten + 1));
            MovePreferredTileAction.MoveToPreferredTile(cultureTurnInfo);
        }
        

    }
}
