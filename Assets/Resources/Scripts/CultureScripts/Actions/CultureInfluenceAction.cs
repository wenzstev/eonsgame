using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureInfluenceAction : CultureTurnInfo
{
    public CultureInfluenceAction(Culture c) : base(c) {}

    public override Turn ExecuteTurn()
    {
        CultureHandler cultureHandler = Culture.CultureHandler;

        foreach(Culture c in cultureHandler.GetAllSettledCultures())
        {
            if (c == Culture) continue;
            if(Culture.CanMerge(c))
            {
                MergeCulture(c);
            }
            else 
            {
                InfluenceCulture(c);
            }
        }
        return turn;
    }

    void MergeCulture(Culture other)
    {
        // duplicated code x1. if happens again, pull out into static helper method
        float percentThisPopulation = (float)Culture.Population / (Culture.Population + other.maxPopTransfer);
        Color lerpedColor = Color.Lerp(Culture.Color, other.Color, percentThisPopulation);

        Turn.AddUpdate(CultureUpdateGetter.GetColorUpdate(this, Culture, lerpedColor));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, Culture, other.Population));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, other, -other.Population));
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, other, Culture.State.PendingRemoval));
        Turn.AddUpdate(CultureUpdateGetter.GetNameUpdate(this, Culture, Culture.CombineStrings(Culture.Name, other.Name)));

    }

    void InfluenceCulture(Culture other)
    {
        //Debug.Log("in influenceculture");
        float influenceValue = Random.value * Culture.influenceRate;
        Color lerpedColor = Color.Lerp(other.Color, Culture.Color, influenceValue);
        Turn.AddUpdate(CultureUpdateGetter.GetColorUpdate(this, other, lerpedColor));
        //EventManager.TriggerEvent("PauseSpeed", null);
    }


}
