using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionMaker
{
    Culture culture;

    public DecisionMaker(Culture c)
    {
        culture = c;
    }

    public Turn ExecuteTurn()
    {
        CultureAction action = new DoNothingAction(culture);


        switch (culture.currentState)
        {
            case Culture.State.Default:
                action = new DefaultAction(culture);
                break;
            case Culture.State.Invaded:
                action = new AttemptRepelAction(culture);
                break;
            case Culture.State.Repelled:
                action = new RepelledAction(culture);
                break;
            case Culture.State.NewOnTile:
                action = new MergeAction(culture);
                break;
            case Culture.State.Overpopulated:
                action = new OverpopulationAction(culture);
                break;
            case Culture.State.PendingRemoval:
            default:
                break;
        }

        return action.ExecuteTurn();
    }
}