using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptRepelAction : CultureAction
{
    public AttemptRepelAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return AttemptRepel();
    }

    Turn AttemptRepel()
    {
        foreach (Culture c in culture.tileInfo.orderToRemoveCulturesIn)
        {
            if (c.currentState == Culture.State.Invader)
            {
                // ability to repel is function of population and affinity (and later tech)
                float hasAffinityAdvantage = c.affinity == culture.affinity ? 0 : .2f;
                float popAdvantage = ((float)culture.population - c.population) / 10f;
                float repelThreshold = .6f + hasAffinityAdvantage + popAdvantage;
                Debug.Log("repel threshold = .6 + " + hasAffinityAdvantage + " + " + popAdvantage);
                if (Random.value < repelThreshold)
                {
                    turn.UpdateCulture(c).newState = Culture.State.Repelled;
                    //Debug.Log(c.name + " is repelled by " + culture.name);
                }
                else
                {
                    turn.UpdateCulture(c).newState = Culture.State.Default;
                    //Debug.Log(c.tile.name);
                    //EventManager.TriggerEvent("PauseSpeed", null);
                }
                if(Random.value < .01f)
                {
                    turn.UpdateCulture(culture).popChange -= 1; // killed in repelling effort
                }
                break;
            }
        }
        turn.UpdateCulture(culture).newState = Culture.State.Default;
        return turn;
    }
}
