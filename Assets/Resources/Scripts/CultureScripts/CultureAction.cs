using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureAction
{
    protected Culture culture;
    public Turn turn;

    protected CultureAction(Culture c)
    {
        //Debug.Log("starting turn for " + c.GetHashCode());
        culture = c;
        turn = Turn.HookTurn();
    }

    public abstract Turn ExecuteTurn();

}

public class DoNothingAction : CultureAction
{
    public DoNothingAction(Culture c) : base(c) { }
    public override Turn ExecuteTurn()
    {
        return turn;
    }
}

public class DefaultAction : CultureAction
{
    public DefaultAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {

        if (Random.value < culture.spreadChance)
        {
            CultureAction moveTile = new MoveTileAction(culture);
            return moveTile.ExecuteTurn();
        }

        turn.UpdateCulture(culture).newState = culture.currentState;

        if(culture.tileInfo.currentMaxPopulation < culture.tileInfo.tilePopulation)
        {
            turn.UpdateCulture(culture).popChange--;
        }


        if (Random.value < culture.growPopulationChance)
        {
            turn.UpdateCulture(culture).popChange++;
        }

        if (Random.value < culture.gainAffinityChance)
        {
            turn.UpdateCulture(culture).newAffinity = culture.tileInfo.tileType;
        }

        return turn;


    }
}


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
            case Culture.State.PendingRemoval:
            default:
                break;
        }

        return action.ExecuteTurn();
    }
}

public class Turn
{
    public Dictionary<Culture, CultureTurnUpdate> turnUpdates { get; private set; }

    static Turn currentTurn;
    bool hasBeenPushed = false;

    Turn()
    {
        turnUpdates = new Dictionary<Culture, CultureTurnUpdate>();
    }

    public static Turn HookTurn()
    {
        if(currentTurn == null || currentTurn.hasBeenPushed)
        {
            //Debug.Log("getting new turn");
            currentTurn = new Turn();
        }
        //Debug.Log("hooking current turn");
        return currentTurn;
    }

    public CultureTurnUpdate UpdateCulture(Culture c)
    {
        CultureTurnUpdate potentialUpdate;
        if(turnUpdates.TryGetValue(c, out potentialUpdate))
        {
            return potentialUpdate;
        }
        turnUpdates.Add(c, new CultureTurnUpdate(c));
        return turnUpdates[c];
    }

    public void UpdateAllCultures()
    {
        //Debug.Log("updating culture. setting pushed to true");
        hasBeenPushed = true;
        foreach(KeyValuePair<Culture, CultureTurnUpdate> c in turnUpdates)
        {
            c.Key.UpdateForTurn(c.Value);
        }
    }

    


}

public class CultureTurnUpdate
{
    public Culture culture;
    public Culture.State newState;
    public int popChange = 0;
    public int techChange = 0;
    public string newAffinity = "";
    public Color newColor;
    public Tile newTile;
    public string newName;

    public CultureTurnUpdate(Culture c)
    {
        culture = c;
        newState = c.currentState;
        newColor = c.mutateColor(c.color); // mutate color slightly every turn
    }
}
