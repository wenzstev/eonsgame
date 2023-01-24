using UnityEngine;

public static class GatherFoodAction
{
    public static float GatherFood(CultureTurnInfo cultureTurnInfo)
    {
        Culture Culture = cultureTurnInfo.Culture;
        CultureFoodStore CultureFoodStore = Culture.CultureFoodStore;

        TileFood TileFood = Culture.Tile.TileFood;


        float differenceBetweenMaxFoodAndCurFood = CultureFoodStore.MaxFoodStore - GetAmountAfterActionCost(CultureFoodStore, cultureTurnInfo.GetCost());
        float maxThatCouldBeGathered = GetMaxThatCouldBeGathered(TileFood, Culture);

        if (maxThatCouldBeGathered == 0 || differenceBetweenMaxFoodAndCurFood < 0) return 0; // no food can be gathered; either no food on tile or pop has too much already

        float ActualAmountToGather = differenceBetweenMaxFoodAndCurFood < maxThatCouldBeGathered ? differenceBetweenMaxFoodAndCurFood : maxThatCouldBeGathered;

        TileFood.CurFood -= ActualAmountToGather; // TODO: update tile food to be it's own turn system
        return ActualAmountToGather;

        //Debug.Log("Affinity rate was " + GetAndInformAffinity());
    }

    static float GetMaxThatCouldBeGathered(TileFood tileFood, Culture culture)
    {
        return tileFood.CurFood * culture.FoodGatherRate * culture.Population * GetAndInformAffinity(culture.Tile.TileChars, culture);
    }

    static float GetAmountAfterActionCost(CultureFoodStore CultureFood, int actionCost)
    {
        return Mathf.Max(0, CultureFood.CurrentFoodStore - actionCost); // can't be less than zero
    }

    static float GetAndInformAffinity(TileChars tileChars, Culture culture)
    {
        AffinityManager affinityManager = culture.AffinityManager;

        if (affinityManager != null)
        {
            TileDrawer.BiomeType curBiome = tileChars.Biome;
            affinityManager.HarvestedOnBiome(curBiome);
            return affinityManager.GetAffinity(curBiome);
        }
        return 1;
    }
}