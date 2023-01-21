using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DecisionMaker
{
    Culture culture;

<<<<<<< HEAD
    Action<CultureTurnInfo>[] CultureActions;
=======
    Func<CultureTurnInfo, Turn> CultureActions;
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

    /*
         public enum State
    {
        Default, 
        Repelled,
        Invaded,
        Invader,
<<<<<<< HEAD
=======
        NewCulture,
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
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
<<<<<<< HEAD
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
=======
        CultureActions = new Func<CultureTurnInfo, Turn>[]
        {
            DefaultAction.ExecuteTurn,
            RepelledAction.ExecuteTurn,
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        };
    }

    

<<<<<<< HEAD
    public void ExecuteTurn()
    {
        CultureTurnInfo cultureTurnInfo = new CultureTurnInfo(culture, Turn.CurrentTurn);
        CultureActions[(int)culture.currentState].Invoke(cultureTurnInfo);
        Turn.AddUpdate(CultureUpdateGetter.GetFoodUpdate(cultureTurnInfo, culture, -cultureTurnInfo.GetCost()));
=======
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
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }
}

public struct CultureTurnInfo
{
    public Culture Culture { get; private set; }
    public int CostPerPop { get; set; }
    public Turn Turn { get; private set; }

<<<<<<< HEAD
    public Tile CurTile { get; private set; }
=======
    public TileComponents TileComponents { get; private set; }
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

    public CultureTurnInfo(Culture c, Turn t)
    {
        Culture = c;
        CostPerPop = 1;
        Turn = t;
<<<<<<< HEAD
        CurTile = Culture.Tile;
=======
        TileComponents = Culture.TileComponents;
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }

    public int GetCost()
    {
        return CostPerPop * Culture.Population;
    }


}