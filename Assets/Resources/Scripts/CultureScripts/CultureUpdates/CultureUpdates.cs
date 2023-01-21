using System;
using UnityEngine;
public abstract class MustInitialize<T>
{
    public MustInitialize(T parameter) { }
}

public interface INonGenericCultureUpdate
{
    object GetCultureChange();
    void ExecuteChange();
    public Culture Target { get; }

}

public static class CultureUpdateGetter
{ 
    public static CultureUpdate<int> GetPopulationUpdate(CultureTurnInfo originator, Culture target, int updateAmount)
    {
        CultureUpdate<int> PopulationUpdate = new CultureUpdate<int>();
        PopulationUpdate.CultureChangeValue = updateAmount;
        PopulationUpdate.Originator = originator;
        PopulationUpdate._target = target;
        PopulationUpdate.ExecuteChangeAction = (populationUpdate, updateAmount) => populationUpdate.Target?.AddPopulation(updateAmount);
        return PopulationUpdate;
    }

    public static CultureUpdate<Culture.State> GetStateUpdate(CultureTurnInfo originator, Culture target, Culture.State newState)
    {
        CultureUpdate<Culture.State> StateUpdate = new CultureUpdate<Culture.State>();
        StateUpdate.CultureChangeValue = newState;
        StateUpdate.Originator = originator;
        StateUpdate._target = target;
        StateUpdate.ExecuteChangeAction = (stateUpdate, newState) => stateUpdate.Target?.ChangeState(newState);
        return StateUpdate;
    }

    public static CultureUpdate<string> GetNameUpdate(CultureTurnInfo originator, Culture target, string newName)
    {
        CultureUpdate<string> GetNameUpdate = new CultureUpdate<string>();
        GetNameUpdate.CultureChangeValue = newName;
        GetNameUpdate.Originator = originator;
        GetNameUpdate._target = target;
        GetNameUpdate.ExecuteChangeAction = (nameUpdate, newName) =>
        {
            nameUpdate.Target?.RenameCulture(newName);
            nameUpdate.Target?.ChangeState(Culture.State.NewOnTile); // renaming resets culture to newontile in order to merge with other named cultures
        };
        return GetNameUpdate;
    }

    public static CultureUpdate<Color> GetColorUpdate(CultureTurnInfo originator, Culture target, Color newColor)
    {
        CultureUpdate<Color> ColorUpdate = new CultureUpdate<Color>();
        ColorUpdate.CultureChangeValue = newColor;
        ColorUpdate.Originator = originator;
        ColorUpdate._target = target;
        ColorUpdate.ExecuteChangeAction = (colorUpdate, newColor) => colorUpdate.Target?.SetColor(newColor);
        return ColorUpdate;
    }

    public static CultureUpdate<Tile> GetTileUpdate(CultureTurnInfo originator, Culture target, Tile newTile)
    {
        CultureUpdate<Tile> TileUpdate = new CultureUpdate<Tile>();
        TileUpdate.CultureChangeValue = newTile;
        TileUpdate.Originator = originator;
        TileUpdate._target = target;
        TileUpdate.ExecuteChangeAction = (tileUpdate, newTile) => tileUpdate.Target?.SetTile(newTile, false);
        return TileUpdate;
    }

    public static CultureUpdate<float> GetFoodUpdate(CultureTurnInfo originator, Culture target, float newFood)
    {
        CultureUpdate<float> FoodUpdate = new CultureUpdate<float>();
        FoodUpdate.CultureChangeValue = newFood;
        FoodUpdate.Originator = originator;
        FoodUpdate._target = target;
        FoodUpdate.ExecuteChangeAction = (fu, nf) =>
        {
            fu.Target?.GetComponent<CultureFoodStore>().AlterFoodStore(nf);
        };
        return FoodUpdate;
    }

    public static CultureUpdate<AffinityStats> GetFullAffinityUpdate(CultureTurnInfo originator, Culture target, AffinityStats newAffinities)
    {
        CultureUpdate<AffinityStats> AffinityUpdate = new CultureUpdate<AffinityStats>();
        AffinityUpdate.CultureChangeValue = newAffinities;
        AffinityUpdate.Originator = originator;
        AffinityUpdate._target = target;
        AffinityUpdate.ExecuteChangeAction = (affinityUpdate, newAffinities) => affinityUpdate.Target?.GetComponent<AffinityManager>().SetStats(newAffinities);
        return AffinityUpdate;
    }

    public static CultureUpdate<Tile> GetMoveUpdate(CultureTurnInfo originator, Culture target, Tile TileToMoveTo)
    {
        CultureUpdate<Tile> MoveUpdate = new CultureUpdate<Tile>();
        MoveUpdate.CultureChangeValue = TileToMoveTo;
        MoveUpdate.Originator = originator;
        MoveUpdate._target = target;
        MoveUpdate.ExecuteChangeAction = (moveUpdate, tileToMoveTo) =>
        {
            if(moveUpdate.Target != null) CultureMoveAction.ExecuteMove(moveUpdate.Originator, tileToMoveTo);
        };
  
        return MoveUpdate;
    }


}



public struct CultureUpdate<G> : INonGenericCultureUpdate
{
    public G CultureChangeValue;

    object INonGenericCultureUpdate.GetCultureChange()
    {
        return CultureChangeValue;
    }

    public CultureTurnInfo Originator;

    public Action<CultureUpdate<G>, G> ExecuteChangeAction; 


    public Culture _target;
    public Culture Target
    {
        get
        {
            if (_target != null && !_target.Equals(null) && _target.isActiveAndEnabled) return _target; // destroyed objects don't immediately equal null
            return null;
        }
    }

    public override string ToString()
    {
        return $"{this.GetType()}({this.GetHashCode()}), Creator: {Originator}, Target: {Target}, Value:{CultureChangeValue}";
    }

    public void ExecuteChange()
    {
        ExecuteChangeAction(this, CultureChangeValue);
    }
}

