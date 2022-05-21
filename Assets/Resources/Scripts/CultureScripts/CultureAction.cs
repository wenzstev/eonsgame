using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICultureAction 
{
    Culture.State ExecuteTurn();
}

public abstract class CultureMoveAction 
{
    protected GameObject prospectiveTile;
    protected Culture culture;

    protected CultureMoveAction(Culture c)
    {
        culture = c;
        prospectiveTile = c.tile.GetRandomNeighbor();
    }


    protected IEnumerator MoveTile(GameObject cultureObj, GameObject tileObj)
    {
        yield return null;
    }
}

public class MoveTileAction : CultureMoveAction, ICultureAction
{
    public MoveTileAction(Culture c) : base(c) {}
  
    public Culture.State ExecuteTurn()
    {
        return AttemptMove();
    }

    Culture.State AttemptMove()
    {

        if (culture.population > culture.maxPopTransfer)
        {
            GameObject splitCulture = culture.SplitCultureFromParent();
            MoveTileAction childMoveAction = new MoveTileAction(splitCulture.GetComponent<Culture>());
            Culture.State newState = childMoveAction.ExecuteTurn();
            if (newState != Culture.State.Moving)
            {
                splitCulture.GetComponent<Culture>().MergeWith(culture);
            }
            return newState;
        }

        if (culture.affinity == prospectiveTile.GetComponent<TileInfo>().tileType || Random.value < .01f)
        {

            culture.StartCoroutine(culture.MoveTile(culture.gameObject, prospectiveTile));
            return Culture.State.Moving;
        }
        return Culture.State.Default;
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
        turn = new Turn();
        ExecuteBeginningTurnFunctions();


        switch (culture.currentState)
        {
            case Culture.State.StartMove:
                ExecuteMovingTurn();
                break;
            case Culture.State.Default:
                ExecuteDefaultTurn();
                break;
            default:
                break;
        }

        return turn;
    }

    void ExecuteBeginningTurnFunctions()
    {
        turn.newColor = culture.mutateColor(culture.color);
    }

    void ExecuteDefaultTurn()
    {
        turn.newState = culture.currentState;
        if (Random.value < culture.growPopulationChance && culture.maxOnTile <= culture.population)
        {
            turn.popChange++;
        }
        if(Random.value < culture.spreadChance)
        {
            turn.newState = Culture.State.StartMove;


        }

    }

    void ExecuteMovingTurn()
    {
        turn.action = new MoveTileAction(culture);
        turn.newState = turn.action.ExecuteTurn();
    }
}

public class Turn
{
    public Culture.State newState;
    public ICultureAction action;
    public int popChange = 0;
    public int techChange = 0;
    public string newAffinity = "";
    public Color newColor;

    public void pushChangesToCulture(Culture c)
    {
        c.AddPopulation(popChange);
        c.SetColor(newColor);
        c.currentState = newState;


    }

}
