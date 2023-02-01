using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class AttemptRepelAction
{

    static float repelModifier = .6f;
    static float dieoffPercent = .02f;

    public static void AttemptRepel(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        foreach (Culture c in culture.Tile.GetComponentInChildren<CultureHandler>().GetAllSettledCultures())
        {
            if (c.currentState == Culture.State.Invader )
            {
                bool didRepel = AttemptRepelInvader(cultureTurnInfo, c);
                if (didRepel) return;
            }
        }
        Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, culture, Culture.State.Default));
    }

    static bool AttemptRepelInvader(CultureTurnInfo cultureTurnInfo, Culture invader)
    {
        Culture culture = cultureTurnInfo.Culture;
        // ability to repel is function of population and affinity (and later tech)
        //TODO: re-add affinity information so that repel ability is function of new affinity
        float hasAffinityAdvantage = 0;
        float popAdvantage = ((float)culture.Population - invader.Population) / 10f;
        float repelThreshold = repelModifier + hasAffinityAdvantage + popAdvantage;
        //Debug.Log("repel threshold = .6 + " + hasAffinityAdvantage + " + " + popAdvantage);
        if (Random.value < repelThreshold)
        {
            Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, invader, Culture.State.Repelled));
            Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, invader, -Mathf.CeilToInt(invader.Population * dieoffPercent)));
            //Debug.Log(invader.name + " is repelled by " + culture.name);
            return true;
        }
        else
        {
            Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, invader, Culture.State.Default));
            Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, culture, -Mathf.CeilToInt(culture.Population * dieoffPercent))); ;


            //Debug.Log(invader.Tile.name);
            //EventManager.TriggerEvent("PauseSpeed", null);
            return false;
        }
    }
}
