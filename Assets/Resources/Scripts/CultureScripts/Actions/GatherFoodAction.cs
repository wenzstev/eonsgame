using UnityEngine;

public class GatherFoodAction : CultureAction
{
    AffinityManager affinityManager;
    TileFood CurrentTileFood;
    CultureFoodStore CultureFood;
    public GatherFoodAction(Culture c) : base(c) 
    {
        affinityManager = c.GetComponent<AffinityManager>();
        CurrentTileFood = Culture.Tile.GetComponent<TileFood>();
        CultureFood = Culture.GetComponent<CultureFoodStore>();
    }

    public override Turn ExecuteTurn()
    {
        GatherFood();
        return turn;
    }

    float GetMaxThatCouldBeGathered()
    {
        return CurrentTileFood.CurFood * Culture.FoodGatherRate * Culture.Population * GetAndInformAffinity();
    }

    float GetMaxFoodStore()
    {
        return CultureFood.MaxFoodStore;
    }

    float GetAmountAfterActionCost()
    {
        return Mathf.Max(0, CultureFood.CurrentFoodStore - GetActionCost()); // can't be less than zero
    }

    void GatherFood()
    {
        float differenceBetweenMaxFoodAndCurFood = GetMaxFoodStore() - GetAmountAfterActionCost();
        float maxThatCouldBeGathered = GetMaxThatCouldBeGathered();

        float ActualAmountToGather = differenceBetweenMaxFoodAndCurFood < maxThatCouldBeGathered ? differenceBetweenMaxFoodAndCurFood : maxThatCouldBeGathered;


        CurrentTileFood.CurFood -= ActualAmountToGather; // TODO: update tile food to be it's own turn system
        Turn.AddUpdate(new FoodUpdate(this, Culture, ActualAmountToGather));
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