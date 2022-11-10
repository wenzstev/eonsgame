

public class GatherFoodAction : CultureAction
{
    public GatherFoodAction(Culture c) : base(c) {}

    public override Turn ExecuteTurn()
    {
        TileFood CurrentTileFood = culture.Tile.GetComponent<TileFood>();
        float GatheredFood = CurrentTileFood.CurFood * culture.FoodGatherRate * .01f;
        turn.UpdateCulture(culture).FoodChange += GatheredFood;
        return turn;
    }

}