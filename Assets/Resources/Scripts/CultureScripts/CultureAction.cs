using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CultureAction
{
    protected Culture culture;

    protected CultureAction(Culture c)
    {
        culture = c;
    }

    public abstract Culture.State ExecuteTurn();

}

public abstract class CultureMoveAction : CultureAction
{
    protected GameObject prospectiveTile;


    protected CultureMoveAction(Culture c) : base(c)
    {
        Debug.Log("tile is " + c.tile);
        prospectiveTile = c.tile.GetRandomNeighbor();
    }

}

public class MoveTileAction : CultureMoveAction
{
    public MoveTileAction(Culture c) : base(c) {}
  
    public override Culture.State ExecuteTurn()
    {
        return AttemptMove();
    }

    Culture.State AttemptMove()
    {
       
        if(prospectiveTile == null || !(culture.affinity == prospectiveTile.GetComponent<TileInfo>().tileType || Random.value < .01f))
        {
            return Culture.State.Default;
        }

        if(culture.population > culture.maxPopTransfer)
        {
            GameObject splitCulture = culture.SplitCultureFromParent();
            culture.StartCoroutine(culture.MoveTile(splitCulture, prospectiveTile));
            splitCulture.GetComponent<Culture>().currentState = Culture.State.Moving;
            return Culture.State.Default;
        }

        culture.StartCoroutine(culture.MoveTile(culture.gameObject, prospectiveTile));
        Debug.Log("setting state to moving for " + culture.GetHashCode());
        return Culture.State.Moving;
        
    }
}



public class DoNothingAction : CultureAction
{
    public DoNothingAction(Culture c) : base(c) { }
    public override Culture.State ExecuteTurn()
    {
        return culture.currentState;
    }
}

public class DecisionMaker
{
    Culture culture;
    Turn turn;

    public DecisionMaker(Culture c)
    {
        culture = c;
    }

    public Turn ExecuteTurn()
    {
        turn = new Turn(culture);
        Debug.Log("executing turn for " + culture.GetHashCode());

        switch (culture.currentState)
        {
            case Culture.State.Default:
                ExecuteDefaultTurn();
                break;
            default:
                break;
        }

        turn.newState = turn.action.ExecuteTurn();
        return turn;
    }



    void ExecuteDefaultTurn()
    {
        Debug.Log("executing default turn for " + culture.GetHashCode());
        turn.newState = culture.currentState;
        if (Random.value < culture.growPopulationChance && culture.population <= culture.maxOnTile)
        {
            turn.popChange++;
        }
        if (Random.value < culture.spreadChance)
        {
            turn.action = new MoveTileAction(culture);
        }
        if(Random.value < culture.gainAffinityChance)
        {
            turn.newAffinity = culture.tileInfo.tileType;
        }


    }

}

public class Turn
{
    public Culture culture;
    public Culture.State newState;
    public CultureAction action;
    public int popChange = 0;
    public int techChange = 0;
    public string newAffinity = "";
    public Color newColor;

    public Turn(Culture c)
    {
        culture = c;
        newState = c.currentState;
        action = new DoNothingAction(c);
        newAffinity = c.affinity;
        newColor = c.mutateColor(c.color); // mutate color slightly every turn
    }


    public void pushChangesToCulture(Culture c)
    {
        Debug.Log("newstate is " + newState + " for " + c.GetHashCode());
        c.AddPopulation(popChange);
        c.SetColor(newColor);
        c.currentState = newState;
        c.affinity = newAffinity;
    }

}
