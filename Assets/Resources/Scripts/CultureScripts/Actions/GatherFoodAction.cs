using UnityEngine;

public class GatherFoodAction : CultureAction
{
    AffinityManager affinityManager;
    TileFood CurrentTileFood;
    public GatherFoodAction(Culture c) : base(c) 
    {
        affinityManager = c.GetComponent<AffinityManager>();
        CurrentTileFood = Culture.Tile.GetComponent<TileFood>();
    }

    public override Turn ExecuteTurn()
    {
        GatherFood();
        return turn;
    }

    void GatherFood()
    {
        float GatheredFood = CurrentTileFood.CurFood * Culture.FoodGatherRate * Culture.Population * GetAndInformAffinity();
        CurrentTileFood.CurFood -= GatheredFood;
        turn.UpdateCulture(Culture).FoodChange += GatheredFood;
        //Debug.Log("Affinity rate was " + GetAndInformAffinity());
    }

    float GetAndInformAffinity()
    {
        if (affinityManager != null)
        {
            affinityManager.HarvestedOnBiome(CurrentTileFood.GetComponent<TileChars>().Biome);
            return affinityManager.GetAffinity(CurrentTileFood.GetComponent<TileChars>().Biome);
        }
        return 1;
    }
}