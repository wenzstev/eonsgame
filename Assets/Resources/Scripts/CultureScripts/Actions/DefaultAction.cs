using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultAction 
{

    public static void ExecuteTurn(CultureTurnInfo cultureTurnInfo)
    {
        Culture Culture = cultureTurnInfo.Culture;

        // culture is on tile, nothing terrible is happening. what will it do? 
        // gather food most likely. everything else is kind of a side effect
        // try to gather food. if no food, move? 

        float amountGathered = AttemptToGatherFood(cultureTurnInfo);

        // tried to gather food, now perform logic

        CheckIfHasSufficientFood(cultureTurnInfo, amountGathered);
        AddSideEffects(cultureTurnInfo);
    }

    static float AttemptToGatherFood(CultureTurnInfo cultureTurnInfo)
    {
        float amountGathered = GatherFoodAction.GatherFood(cultureTurnInfo);
        Turn.AddUpdate(CultureUpdateGetter.GetFoodUpdate(cultureTurnInfo, cultureTurnInfo.Culture, amountGathered));

        return amountGathered;
    }

    static void CheckIfHasSufficientFood(CultureTurnInfo cultureTurnInfo, float amountGathered)
    {
        Culture Culture = cultureTurnInfo.Culture;
        CultureFoodStore CultureFoodStore = Culture.CultureFoodStore;

        if (amountGathered - cultureTurnInfo.GetCost() > 0) return; // stay in default mode if you're gathering enough food

        float starvingThreshold = .01f;
        float overpopulationThreshold = .05f;
        float seekingFoodThreshold = .2f;
        
        float FoodStore = CultureFoodStore.CurrentFoodStore;
        float MaxFoodStore = CultureFoodStore.MaxFoodStore;

        Culture.State newState = FoodStore < starvingThreshold * MaxFoodStore ? Culture.State.Starving :
            FoodStore < overpopulationThreshold * MaxFoodStore ? 
            Culture.State.Overpopulated : 
            FoodStore < seekingFoodThreshold * MaxFoodStore ?
            Culture.State.SeekingFood : Culture.State.Default;

        

        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, Culture, newState));
    }


    static void AddSideEffects(CultureTurnInfo cultureTurnInfo)
    {
        Culture Culture = cultureTurnInfo.Culture;

        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, Culture, GrowPopulation(Culture)));

        // need to change influence into a side effect?
        if (Culture.CultureHandler.GetAllSettledCultures().Length > 1 && Random.value < .1f)
        {
            //Debug.Log("influencing neighbors");
            CultureInfluenceAction.ExecuteTurn(cultureTurnInfo);
        }

        Turn.AddUpdate(CultureUpdateGetter.GetColorUpdate(cultureTurnInfo, Culture, Culture.mutateColor(Culture.Color)));

    }

    static int GrowPopulation(Culture Culture)
    {
        if (Culture.Population == 1) return 0; // can't reproduce if only one person
        float combinedFertilityRate = Culture.FertilityRate * Culture.Population;
        return Random.value < combinedFertilityRate ? 1 : 0;
    }

}