using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct Turn
{
    static Turn currentTurn = InitializeTurn();

    static Turn InitializeTurn()
    {
        Turn firstTurn = new Turn();
        firstTurn.UpdateHolder = new UpdateHolder();
        return firstTurn;
    }

    public enum TurnState
    {
        Next,
        Executing,
        Complete
    };

    public static Turn CurrentTurn
    {
        get {
            if (currentTurn.CurState != TurnState.Next)
            {
                currentTurn = new Turn(currentTurn.UpdateHolder);
            }
            //Debug.Log("hooking current turn");
            return currentTurn;
        }
    }

    public TurnState CurState { get; private set; }

    public UpdateHolder UpdateHolder { get; private set;}

    public static void UpdateAllCultures()
    {
        Turn cur = CurrentTurn;
        cur.CurState = TurnState.Executing;
        cur.UpdateHolder.ExecuteAllUpdates();
        cur.CurState = TurnState.Complete;
    }


    Turn(UpdateHolder holder)
    {
        UpdateHolder = holder;
        CurState = TurnState.Next;
    }

    public static void AddIntUpdate(CultureUpdate<int> update)
    {
        CurrentTurn.UpdateHolder.AddIntUpdate(update);
    }
    public static void AddFloatUpdate(CultureUpdate<float> update)
    {
        CurrentTurn.UpdateHolder.AddFloatUpdate(update);
    }
    public static void AddStringUpdate(CultureUpdate<string> update)
    {
        CurrentTurn.UpdateHolder.AddStringUpdate(update);
    }
    public static void AddTileUpdate(CultureUpdate<Tile> update)
    {
        CurrentTurn.UpdateHolder.AddTileUpdate(update);
    }
    public static void AddAffinityUpdate(CultureUpdate<AffinityStats> update)
    {
        CurrentTurn.UpdateHolder.AddAffinityUpdate(update);
    }
    public static void AddColorUpdate(CultureUpdate<Color> update)
    {
        CurrentTurn.UpdateHolder.AddColorUpdate(update);
    }
    public static void AddStateUpdate(CultureUpdate<Culture.State> update)
    {
        CurrentTurn.UpdateHolder.AddStateUpdate(update);
    }
}




