

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
        float GatheredFood = CurrentTileFood.CurFood * culture.FoodGatherRate * culture.Population * .01f * getAffinityModifier();
        CurrentTileFood.CurFood -= GatheredFood;
        turn.UpdateCulture(culture).FoodChange += GatheredFood;
    }

    float getAffinityModifier()
    {
        if(affinityManager != null) return affinityManager.GetAffinity(CurrentTileFood.GetComponent<TileInfo>().tileType);
        return 1;
    }
}