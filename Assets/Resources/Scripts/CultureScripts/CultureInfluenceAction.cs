using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureInfluenceAction : CultureAction
{
    public CultureInfluenceAction(Culture c) : base(c) {}

    public override Turn ExecuteTurn()
    {
        TileInfo ti = culture.tileInfo;

        foreach(Culture c in ti.cultures.Values)
        {
            if (c == culture) continue;
            if(culture.CanMerge(c))
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
        float percentThisPopulation = (float)culture.Population / (culture.Population + other.maxPopTransfer);
        turn.UpdateCulture(culture).newColor = Color.Lerp(culture.Color, other.Color, percentThisPopulation);
        turn.UpdateCulture(culture).popChange += other.Population;
        turn.UpdateCulture(other).popChange -= other.Population;
        turn.UpdateCulture(other).newState = Culture.State.PendingRemoval;
    }

    void InfluenceCulture(Culture other)
    {
        //Debug.Log("in influenceculture");
        float influenceValue = Random.value * culture.influenceRate;
        turn.UpdateCulture(other).newColor = Color.Lerp(other.Color, culture.Color, influenceValue);
        //EventManager.TriggerEvent("PauseSpeed", null);
    }


}
