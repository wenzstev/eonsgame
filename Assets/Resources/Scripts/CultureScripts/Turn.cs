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
            Debug.Log("updating " + c.Key.name + " in turn " + turnNumber);
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
