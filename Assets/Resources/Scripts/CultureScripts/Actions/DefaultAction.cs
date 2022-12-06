using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAction : CultureAction
{
    public DefaultAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {

        // culture is on tile, nothing terrible is happening. what will it do? 
        // gather food most likely. everything else is kind of a side effect
        // try to gather food. if no food, move? 

        AttemptToGatherFood();

        // tried to gather food, now perform logic

        CheckIfHasSufficientFood();
        AddSideEffects();

        return turn;
    }

    void AttemptToGatherFood()
    {
        if(Culture.CultureFoodStore.CurrentFoodStore < Culture.CultureFoodStore.MaxFoodStore)
        {
            GatherFoodAction gather = new GatherFoodAction(Culture);
            gather.ExecuteTurn();
        }
    }

    void CheckIfHasSufficientFood()
    {
        float FoodChange = turn.turnUpdates[Culture].FoodChange;
        float FoodStore = Culture.GetComponent<CultureFoodStore>().CurrentFoodStore;
        float MaxFoodStore = Culture.GetComponent<CultureFoodStore>().MaxFoodStore;
        turn.UpdateCulture(Culture).newState = FoodStore + FoodChange < Culture.Population 
            ? Culture.State.Overpopulated : 
            FoodStore < MaxFoodStore / 2f && FoodChange < 0 ? 
            Culture.State.SeekingFood : Culture.State.Default;
    }



    void AddSideEffects()
    {
        turn.UpdateCulture<PopulationUpdate, int>(Culture, GrowPopulation());

        // need to change influence into a side effect?
        if (Culture.tileInfo.cultures.Count > 1 && Random.value < .1f)
        {
            //Debug.Log("influencing neighbors");
            CultureInfluenceAction influenceNeighbors = new CultureInfluenceAction(Culture);
            influenceNeighbors.ExecuteTurn();
        }

    }

    int GrowPopulation()
    {
        if (Culture.Population == 1) return 0; // can't reproduce if only one person
        float combinedFertilityRate = Culture.FertilityRate * Culture.Population;
        return Random.value < combinedFertilityRate ? 1 : 0;
    }

}