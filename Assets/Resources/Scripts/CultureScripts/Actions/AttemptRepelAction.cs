using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptRepelAction : CultureTurnInfo
{
    public AttemptRepelAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return AttemptRepel();
    }

    Turn AttemptRepel()
    {
        foreach (Culture c in Culture.tileInfo.orderToRemoveCulturesIn)
        {
            if (c.currentState == Culture.State.Invader)
            {
                // ability to repel is function of population and affinity (and later tech)
                //TODO: re-add affinity information so that repel ability is function of new affinity
                float hasAffinityAdvantage = 0;
                float popAdvantage = ((float)Culture.Population - c.Population) / 10f;
                float repelThreshold = .6f + hasAffinityAdvantage + popAdvantage;
                //Debug.Log("repel threshold = .6 + " + hasAffinityAdvantage + " + " + popAdvantage);
                if (Random.value < repelThreshold)
                {
                    Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, c, Culture.State.Repelled));
                    Debug.Log(c.name + " is repelled by " + Culture.name);
                }
                else
                {
                    Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, c, Culture.State.Default));

                    Debug.Log(c.Tile.name);
                    //EventManager.TriggerEvent("PauseSpeed", null);
                }
                if(Random.value < .01f)
                {
                    Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, Culture, -1)); // killed in repelling effort
                    //Debug.Log("some of " + Culture.name + " killed in repel");
                }
                break;
            }
        }
        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, Culture, Culture.State.Default));
        return turn;
    }
}
