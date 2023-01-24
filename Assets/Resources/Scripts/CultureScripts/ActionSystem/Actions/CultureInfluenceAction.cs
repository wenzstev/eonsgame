using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CultureInfluenceAction 
{
    public static void ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        CultureHandler cultureHandler = culture.CultureHandler;

        foreach(Culture c in cultureHandler.GetAllSettledCultures())
        {
            if (c == culture) continue;
            if(culture.CanMerge(c))
            {
                MergeCulture(cultureTurnInfo, c);
            }
            else 
            {
                InfluenceCulture(cultureTurnInfo, c);
            }
        }
    }

    static void MergeCulture(CultureTurnInfo cultureTurnInfo, Culture other)
    {
        Culture culture = cultureTurnInfo.Culture;
        // duplicated code x1. if happens again, pull out into static helper method
        float percentThisPopulation = (float)culture.Population / (culture.Population + other.maxPopTransfer);
        Color lerpedColor = Color.Lerp(culture.Color, other.Color, percentThisPopulation);

        Turn.AddColorUpdate(CultureUpdateGetter.GetColorUpdate(cultureTurnInfo, culture, lerpedColor));
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, culture, other.Population));
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, other, -other.Population));
        Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, other, Culture.State.PendingRemoval));
        Turn.AddStringUpdate(CultureUpdateGetter.GetNameUpdate(cultureTurnInfo, culture, Culture.CombineStrings(culture.Name, other.Name)));

    }

    static void InfluenceCulture(CultureTurnInfo cultureTurnInfo, Culture other)
    {
        Culture culture = cultureTurnInfo.Culture;
        //Debug.Log("in influenceculture");
        float influenceValue = Random.value * culture.influenceRate;
        Color lerpedColor = Color.Lerp(other.Color, culture.Color, influenceValue);
        Turn.AddColorUpdate(CultureUpdateGetter.GetColorUpdate(cultureTurnInfo, other, lerpedColor));
        //EventManager.TriggerEvent("PauseSpeed", null);
    }


}
