using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turn
{
    static Turn currentTurn;
<<<<<<< HEAD

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
=======
    public static Turn CurrentTurn
    {
        get {
            if (currentTurn == null || currentTurn.hasBeenPushed)
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
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
<<<<<<< HEAD
    public TurnState CurState { get; private set; }

=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074

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
<<<<<<< HEAD
        List<INonGenericCultureUpdate> UpdateList = CurrentTurn.UpdateList;
        CurrentTurn.CurState = TurnState.Executing;
        foreach(INonGenericCultureUpdate update in UpdateList)
=======
        foreach(INonGenericCultureUpdate update in CurrentTurn.UpdateList)
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        {
            update.ExecuteChange();
        }
        CurrentTurn.hasBeenPushed = true;
<<<<<<< HEAD
        CurrentTurn.CurState = TurnState.Complete;
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }

    public static INonGenericCultureUpdate[] GetPendingUpdatesFor(Culture c)
    {
        return CurrentTurn.UpdateList.Where(u => u.Target == c).ToArray();
    }


    Turn()
    {
        UpdateList = new List<INonGenericCultureUpdate>();
<<<<<<< HEAD
        CurState = TurnState.Next;
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }
}

