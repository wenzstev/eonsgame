using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DecisionMaker
{
    Culture culture;

    Action<CultureTurnInfo>[] CultureActions;

    /*
         public enum State
    {
        Default, 
        Repelled,
        Invaded,
        Invader,
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
        CultureActions = new Action<CultureTurnInfo>[]
        {
            DefaultAction.ExecuteTurn,                          // default
            RepelledAction.RepelCulture,                        // repelled
            AttemptRepelAction.AttemptRepel,                    // invaded
            DefaultAction.ExecuteTurn,                          // invader
            DoNothingAction.DoNothing,                          // moving
            MergeWithTileAction.CombineCultureWithNewTile,      // newontile
            DoNothingAction.DoNothing,                          // pendingremoval
            OverpopulationAction.ExecuteTurn,                   // overpopulatied
            MovePreferredTileAction.MoveToPreferredTile,        // seeking food
            StarvationAction.ExecuteTurn,                       // starving
        };
    }

    

    public void ExecuteTurn()
    {
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(culture, Turn.CurrentTurn);
        CultureActions[(int)culture.currentState].Invoke(cultureTurnInfo);
        Turn.AddFloatUpdate(CultureUpdateGetter.GetFoodUpdate(cultureTurnInfo, culture, -cultureTurnInfo.GetCost()));
    }
}

public struct CultureTurnInfo
{
    public Culture Culture { get; private set; }
    public int CostPerPop { get; set; }
    public Turn Turn { get; private set; }

    public Tile CurTile { get; private set; }

    public CultureTurnInfo(Culture c, Turn t)
    {
        Culture = c;
        CostPerPop = 1;
        Turn = t;
        CurTile = Culture.Tile;
    }

    public int GetCost()
    {
        return CostPerPop * Culture.Population;
    }

}