using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DecisionMaker
{
    Culture culture;

    Func<CultureTurnInfo, Turn> CultureActions;

    /*
         public enum State
    {
        Default, 
        Repelled,
        Invaded,
        Invader,
        NewCulture,
        Moving,
        NewOnTile,
        PendingRemoval,
        Overpopulated,
        SeekingFood,
        Starving
    }
     */

    public DecisionMaker(Culture c)
    {
        culture = c;
        CultureActions = new Func<CultureTurnInfo, Turn>[]
        {
            DefaultAction.ExecuteTurn,
            RepelledAction.ExecuteTurn,
        };
    }

    

    public Turn ExecuteTurn()
    {
        CultureTurnInfo cti = new CultureTurnInfo(culture);

        return CultureActions[(int)culture.currentState].ExecuteTurn();


        CultureTurnInfo action = new DoNothingAction(cti);


        switch (culture.currentState)
        {
            case Culture.State.Default:
            case Culture.State.Invader:
                action = new DefaultAction(culture);
                break;
            case Culture.State.Invaded:
                action = new AttemptRepelAction(culture);
                break;
            case Culture.State.Repelled:
                action = new RepelledAction(culture);
                break;
            case Culture.State.NewOnTile:
                action = new MergeWithTileAction(culture);
                break;
            case Culture.State.Overpopulated:
                action = new OverpopulationAction(culture);
                break;
            case Culture.State.SeekingFood:
                action = new MovePreferredTileAction(culture);
                break;
            case Culture.State.Starving:
                action = new StarvationAction(culture);
                break;
            case Culture.State.PendingRemoval:
            default:
                break;
        }

        return action.ExecuteTurn();
    }
}

public struct CultureTurnInfo
{
    public Culture Culture { get; private set; }
    public int CostPerPop { get; set; }
    public Turn Turn { get; private set; }

    public TileComponents TileComponents { get; private set; }

    public CultureTurnInfo(Culture c, Turn t)
    {
        Culture = c;
        CostPerPop = 1;
        Turn = t;
        TileComponents = Culture.TileComponents;
    }

    public int GetCost()
    {
        return CostPerPop * Culture.Population;
    }


}