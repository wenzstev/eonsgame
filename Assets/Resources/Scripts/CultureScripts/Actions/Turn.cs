using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turn
{
    static Turn currentTurn;

    public enum TurnState
    {
        Executing,
        Next, 
        Complete
    };

    public static Turn CurrentTurn
    {
        get {
            if (currentTurn == null || currentTurn.CurState != TurnState.Next)
            {
                //Debug.Log("getting new turn");
                currentTurn = new Turn();
            }
            //Debug.Log("hooking current turn");
            return currentTurn;
        }
    }

    List<INonGenericCultureUpdate> UpdateList;
    bool hasBeenPushed = false;
    public TurnState CurState { get; private set; }


    /// <summary>
    /// Add a Culture Update to the current turn. Generally advised to create the update when adding.
    /// </summary>
    /// <param name="update">The INonGenericCultureUpdate to add.</param>
    public static void AddUpdate(INonGenericCultureUpdate update)
    {
        //Debug.Log($"Adding {update}");
        CurrentTurn.UpdateList.Add(update);
    }

    public static void UpdateAllCultures()
    {
        List<INonGenericCultureUpdate> UpdateList = CurrentTurn.UpdateList;
        CurrentTurn.CurState = TurnState.Executing;
        foreach(INonGenericCultureUpdate update in UpdateList)
        {
            update.ExecuteChange();
        }
        CurrentTurn.hasBeenPushed = true;
        CurrentTurn.CurState = TurnState.Complete;
    }

    public static INonGenericCultureUpdate[] GetPendingUpdatesFor(Culture c)
    {
        return CurrentTurn.UpdateList.Where(u => u.Target == c).ToArray();
    }


    Turn()
    {
        UpdateList = new List<INonGenericCultureUpdate>();
        CurState = TurnState.Next;
    }
}

