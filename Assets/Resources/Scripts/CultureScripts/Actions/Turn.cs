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


public abstract class MustInitialize<T>
{
    public MustInitialize(T parameter) { }
}

public interface INonGenericCultureUpdate
{
    object GetCultureChange();
    void ExecuteChange();
    public CultureAction Originator { get; }
    public Culture Target { get; }

}

public abstract class CultureUpdate<G> : MustInitialize<CultureAction>, INonGenericCultureUpdate
{
    protected G cultureChangeValue;

    object INonGenericCultureUpdate.GetCultureChange()
    {
        return GetCultureChange();
    }

    protected G GetCultureChange()
    {
        return cultureChangeValue;
    }

    public abstract void ExecuteChange();
    public CultureAction Originator { get; private set; }


    Culture _target;
    public Culture Target { 
        get { 
            if(_target != null && !_target.Equals(null)) return _target; // destroyed objects don't immediately equal null
            return null;
        }
    }

    public CultureUpdate(CultureAction originator, Culture target, G value) : base(originator)
    {
        Originator = originator;
        cultureChangeValue = value;
        _target = target;
    }

    public override string ToString()
    {
        return $"{this.GetType()}({this.GetHashCode()}), Creator: {Originator}, Target: {Target}, Value:{cultureChangeValue}";
    }

}

public class PopulationUpdate : CultureUpdate<int>
{
    public PopulationUpdate(CultureAction originator, Culture target, int val) : base (originator, target, val) { }

    public override void ExecuteChange()
    {
        Target?.AddPopulation(GetCultureChange());
    }
}

public class StateUpdate : CultureUpdate<Culture.State>
{
    public StateUpdate(CultureAction originator, Culture target, Culture.State val) : base(originator, target, val) { }
    public override void ExecuteChange()
    {
        Target?.ChangeState(GetCultureChange());
    }
}

public class NameUpdate : CultureUpdate<string>
{
    public NameUpdate(CultureAction originator, Culture target, string val) : base(originator, target, val) { }
    public override void ExecuteChange()
    {
        Target?.RenameCulture(GetCultureChange());
    }
}

public class ColorUpdate : CultureUpdate<Color>
{
    public ColorUpdate(CultureAction originator, Culture target, Color val) : base(originator, target, val) { }
    public override void ExecuteChange()
    {
        Target?.SetColor(GetCultureChange());
    }
}

public class TileUpdate : CultureUpdate<Tile>
{
    public TileUpdate(CultureAction originator, Culture target, Tile val) : base(originator, target, val) { }
    public override void ExecuteChange()
    {
        Target?.SetTile(GetCultureChange(), false);
    }
}

public class FoodUpdate : CultureUpdate<float>
{
    public FoodUpdate(CultureAction originator, Culture target, float val) : base(originator, target, val) { }
    public override void ExecuteChange()
    {
        Target?.GetComponent<CultureFoodStore>().AlterFoodStore(GetCultureChange());
    }
}