using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public Dictionary<Culture, CultureTurnUpdate> turnUpdates { get; private set; }

    static Turn currentTurn;
    bool hasBeenPushed = false;

    static int turnNumber;

    Turn()
    {
        turnUpdates = new Dictionary<Culture, CultureTurnUpdate>();
    }

    public static Turn HookTurn()
    {
        if (currentTurn == null || currentTurn.hasBeenPushed)
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
        if (turnUpdates.TryGetValue(c, out potentialUpdate))
        {
            return potentialUpdate;
        }
        turnUpdates.Add(c, new CultureTurnUpdate(c));
        return turnUpdates[c];
    }

    public void UpdateAllCultures()
    {
        turnNumber++;
        hasBeenPushed = true;
        foreach (KeyValuePair<Culture, CultureTurnUpdate> c in turnUpdates)
        {
            c.Key.UpdateForTurn(c.Value);
        }
    }

    public void CombineTurns(Turn other)
    {
        foreach (CultureTurnUpdate update in other.turnUpdates.Values)
        {
            CultureTurnUpdate potentialUpdate;
            if(turnUpdates.TryGetValue(update.culture, out potentialUpdate))
            {
                potentialUpdate.MergeUpdates(update);
            }
            else 
            {
                turnUpdates.Add(update.culture, update);
            }
        }
    }
}


public class CultureTurnUpdate
{
    public Culture culture;
    Culture.State _newState;
    public Culture.State newState
    {
        get { return _newState; }
        set {
            //Debug.Log("setting newstate to " + value);
            _newState = value;
        }
    }
    public int popChange = 0;
    public int techChange = 0;
    public int newAffinity = -1;
    public Color newColor;
    public Tile newTile;
    public string newName;

    public CultureTurnUpdate(Culture c)
    {
        culture = c;
        newState = c.currentState;
        newColor = c.mutateColor(c.color); // mutate color slightly every turn
    }

    public void MergeUpdates(CultureTurnUpdate other) 
    {
        if(culture != other.culture)
        {
            Debug.LogError("trying to merge unrelated updates!");
        }
        // values are combined, other takes precedence
        newState = other.newState;
        popChange += other.popChange;
        techChange += other.techChange;
        newAffinity = other.newAffinity;
        newColor = other.newColor;
        newTile = other.newTile;
        newName = other.newName;
    }
}
