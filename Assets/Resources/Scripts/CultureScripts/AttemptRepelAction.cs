using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptRepelAction : CultureAction
{
    public AttemptRepelAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        turn.newState = AttemptRepel();
        return turn;
    }

    Culture.State AttemptRepel()
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
                    c.currentState = Culture.State.Repelled;
                    Debug.Log(c.name + " is repelled by " + culture.name);
                    break;
                }
                if(Random.value < .01f)
                {
                    turn.popChange -= 1; // killed in repelling effort
                }
            }
        }
        return Culture.State.Default;
    }
}
