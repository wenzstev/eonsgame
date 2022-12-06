using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureInfluenceAction : CultureAction
{
    public CultureInfluenceAction(Culture c) : base(c) {}

    public override Turn ExecuteTurn()
    {
        TileInfo ti = Culture.tileInfo;

        foreach(Culture c in ti.cultures.Values)
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
        turn.UpdateCulture(Culture).newColor = Color.Lerp(Culture.Color, other.Color, percentThisPopulation);
        turn.UpdateCulture(Culture).popChange += other.Population;
        turn.UpdateCulture(other).popChange -= other.Population;
        turn.UpdateCulture(other).newState = Culture.State.PendingRemoval;
    }

    void InfluenceCulture(Culture other)
    {
        //Debug.Log("in influenceculture");
        float influenceValue = Random.value * Culture.influenceRate;
        turn.UpdateCulture(other).newColor = Color.Lerp(other.Color, Culture.Color, influenceValue);
        //EventManager.TriggerEvent("PauseSpeed", null);
    }


}
