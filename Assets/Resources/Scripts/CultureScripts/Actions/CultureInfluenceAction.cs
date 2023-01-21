using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
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

        Turn.AddUpdate(CultureUpdateGetter.GetColorUpdate(cultureTurnInfo, culture, lerpedColor));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, culture, other.Population));
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, other, -other.Population));
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, other, Culture.State.PendingRemoval));
        Turn.AddUpdate(CultureUpdateGetter.GetNameUpdate(cultureTurnInfo, culture, Culture.CombineStrings(culture.Name, other.Name)));

    }

    static void InfluenceCulture(CultureTurnInfo cultureTurnInfo, Culture other)
    {
        Culture culture = cultureTurnInfo.Culture;
        //Debug.Log("in influenceculture");
        float influenceValue = Random.value * culture.influenceRate;
        Color lerpedColor = Color.Lerp(other.Color, culture.Color, influenceValue);
        Turn.AddUpdate(CultureUpdateGetter.GetColorUpdate(cultureTurnInfo, other, lerpedColor));
=======
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
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        //EventManager.TriggerEvent("PauseSpeed", null);
    }


}
