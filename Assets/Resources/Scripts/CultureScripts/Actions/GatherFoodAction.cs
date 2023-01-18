using UnityEngine;

public static class GatherFoodAction
{
    AffinityManager affinityManager;
    TileFood CurrentTileFood;
    CultureFoodStore CultureFood;

    public float FoodGatheredInTurn { get; private set; }
    public GatherFoodAction(Culture c) : base(c) 
    {
        affinityManager = c.GetComponent<AffinityManager>();
        CurrentTileFood = Culture.Tile.GetComponent<TileFood>();
        CultureFood = Culture.GetComponent<CultureFoodStore>();
    }

    public static Turn ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        GatherFood();
        return turn;
    }

    static float GetMaxThatCouldBeGathered(TileFood tileFood, Culture culture)
    {
        return tileFood.CurFood * culture.FoodGatherRate * culture.Population * GetAndInformAffinity(culture);
    }


    static float GetAmountAfterActionCost(CultureFoodStore CultureFood, int actionCost)
    {
        return Mathf.Max(0, CultureFood.CurrentFoodStore - actionCost); // can't be less than zero
    }

    public static float GatherFood(CultureTurnInfo cultureTurnInfo)
    {
        Culture Culture = cultureTurnInfo.Culture;
        CultureFoodStore CultureFoodStore = Culture.CultureFoodStore;

        TileFood TileFood = Culture.


        float differenceBetweenMaxFoodAndCurFood = CultureFoodStore.MaxFoodStore - GetAmountAfterActionCost(CultureFoodStore, cultureTurnInfo.GetCost());
        float maxThatCouldBeGathered = GetMaxThatCouldBeGathered();

        if (maxThatCouldBeGathered == 0 || differenceBetweenMaxFoodAndCurFood < 0) return; // no food can be gathered; either no food on tile or pop has too much already

        float ActualAmountToGather = differenceBetweenMaxFoodAndCurFood < maxThatCouldBeGathered ? differenceBetweenMaxFoodAndCurFood : maxThatCouldBeGathered;

        CurrentTileFood.CurFood -= ActualAmountToGather; // TODO: update tile food to be it's own turn system
        FoodGatheredInTurn = ActualAmountToGather;
        return ActualAmountToGather;

        //Debug.Log("Affinity rate was " + GetAndInformAffinity());
    }

    static float GetAndInformAffinity(Culture culture)
    {
        AffinityManager affinityManager = culture.AffinityManager;

        if (affinityManager != null)
        {
            affinityManager.HarvestedOnBiome(CurrentTileFood.GetComponent<TileChars>().Biome);
            return affinityManager.GetAffinity(CurrentTileFood.GetComponent<TileChars>().Biome);
        }
        return 1;
    }
}