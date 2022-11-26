using UnityEngine;

public class GatherFoodAction : CultureAction
{
    AffinityManager affinityManager;
    TileFood CurrentTileFood;
    public GatherFoodAction(Culture c) : base(c) 
    {
        affinityManager = c.GetComponent<AffinityManager>();
        CurrentTileFood = culture.Tile.GetComponent<TileFood>();
    }

    public override Turn ExecuteTurn()
    {
        GatherFood();
        return turn;
    }

    void GatherFood()
    {
        float GatheredFood = CurrentTileFood.CurFood * culture.FoodGatherRate * culture.Population * GetAndInformAffinity();
        CurrentTileFood.CurFood -= GatheredFood;
        turn.UpdateCulture(culture).FoodChange += GatheredFood;
        Debug.Log("Affinity rate was " + GetAndInformAffinity());
    }

    float GetAndInformAffinity()
    {
        if (affinityManager != null)
        {
            affinityManager.HarvestedOnBiome(CurrentTileFood.GetComponent<TileInfo>().tileType);
            return affinityManager.GetAffinity(CurrentTileFood.GetComponent<TileInfo>().tileType);
        }
        return 1;
    }
}