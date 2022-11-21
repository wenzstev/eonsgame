

public class GatherFoodAction : CultureAction
{
    public GatherFoodAction(Culture c) : base(c) {}

    public override Turn ExecuteTurn()
    {
        GatherFood();
        return turn;
    }

    void GatherFood()
    {
        TileFood CurrentTileFood = culture.Tile.GetComponent<TileFood>();
        float GatheredFood = CurrentTileFood.CurFood * culture.FoodGatherRate * culture.Population * .01f;
        CurrentTileFood.CurFood -= GatheredFood;
        turn.UpdateCulture(culture).FoodChange += GatheredFood;
    }
}