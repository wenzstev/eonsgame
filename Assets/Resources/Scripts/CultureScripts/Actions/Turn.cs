using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turn
{
    static Turn currentTurn;
    public static Turn CurrentTurn
    {
        get {
            if (currentTurn == null || currentTurn.hasBeenPushed)
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
        foreach(INonGenericCultureUpdate update in CurrentTurn.UpdateList)
        {
            update.ExecuteChange();
        }
        CurrentTurn.hasBeenPushed = true;
    }

    public static INonGenericCultureUpdate[] GetPendingUpdatesFor(Culture c)
    {
        return CurrentTurn.UpdateList.Where(u => u.Target == c).ToArray();
    }


    Turn()
    {
        UpdateList = new List<INonGenericCultureUpdate>();
    }
}

