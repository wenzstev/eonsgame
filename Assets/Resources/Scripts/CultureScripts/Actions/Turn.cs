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

    List<INonGenericCultureUpdate> UpdateList;

    void ph()
    {
        UpdateList = new List<INonGenericCultureUpdate>();
        UpdateList.Add(new PopulationUpdate(null, 3));
        UpdateList.Add(new StateUpdate(null, Culture.State.Default));

        UpdateList[0].GetCultureChange();
        string s = UpdateList[1].UPDATE_TYPE;
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

    public void UpdateCulture(Culture c, CultureUpdate update)
    {
 
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

    public List<INonGenericCultureUpdate> updates;


    public Culture.State newState
    {
        get { return _newState; }
        set {
            //Debug.Log("setting newstate to " + value);
            _newState = value;
        }
    }
    int popChange = 0;
    float FoodChange = 0;
    int newAffinity = -1;
    Color newColor;
    Tile newTile;
    string newName;

    public CultureTurnUpdate(Culture c)
    {
        culture = c;
        newState = c.currentState;
        newColor = c.mutateColor(c.Color); // mutate color slightly every turn
    }

    public void AddPopChange(int addition)
    {
        popChange += addition;
    }

    public int GetPopChange()
    {
        return popChange;
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
        newAffinity = other.newAffinity;
        newColor = other.newColor;
        newTile = other.newTile;
        newName = other.newName;
    }
}


public abstract class MustInitialize<T>
{
    public MustInitialize(T parameter) { }
}

public interface INonGenericCultureUpdate
{
    object GetCultureChange();
    string UPDATE_TYPE { get; }
    void ExecuteChange();
}

public abstract class CultureUpdate<G> : MustInitialize<CultureAction>, INonGenericCultureUpdate
{
    G cultureChangeValue;

    object INonGenericCultureUpdate.GetCultureChange()
    {
        return GetCultureChange();
    }

    protected G GetCultureChange()
    {
        return cultureChangeValue;
    }

    public abstract void ExecuteChange();

    public abstract string UPDATE_TYPE { get; }
    public CultureAction Originator { get; }

    public Culture Target { get { return Originator.Culture; } }
    public CultureUpdate(CultureAction originator, G value) : base(originator)
    {
        Originator = originator;
        cultureChangeValue = value;
    }
}

public class PopulationUpdate : CultureUpdate<int>
{
    public override string UPDATE_TYPE {get { return "POPULATION";}}
    public PopulationUpdate(CultureAction originator, int val) : base (originator, val) { }

    public override void ExecuteChange()
    {
        Target.AddPopulation(GetCultureChange());
    }
}

public class StateUpdate : CultureUpdate<Culture.State>
{
    public override string UPDATE_TYPE { get { return "STATE"; } }
    public StateUpdate(CultureAction originator, Culture.State val) : base(originator, val) { }
}