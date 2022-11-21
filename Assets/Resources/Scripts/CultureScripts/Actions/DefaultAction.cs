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
        if(culture.CultureFoodStore.CurrentFoodStore < culture.CultureFoodStore.MaxFoodStore)
        {
            GatherFoodAction gather = new GatherFoodAction(culture);
            gather.ExecuteTurn();
        }
    }

    void CheckIfHasSufficientFood()
    {
        float FoodChange = turn.turnUpdates[culture].FoodChange;
        float FoodStore = culture.GetComponent<CultureFoodStore>().CurrentFoodStore;
        float MaxFoodStore = culture.GetComponent<CultureFoodStore>().MaxFoodStore;
        turn.UpdateCulture(culture).newState = FoodStore + FoodChange < culture.Population 
            ? Culture.State.Overpopulated : 
            FoodStore < MaxFoodStore / 2f && FoodChange < 0 ? 
            Culture.State.SeekingFood : Culture.State.Default;
    }



    void AddSideEffects()
    {
        turn.UpdateCulture(culture).popChange += GrowPopulation();

        if (Random.value < culture.gainAffinityChance)
        {
            turn.UpdateCulture(culture).newAffinity = (int)culture.tileInfo.tileType;
        }

        // need to change influence into a side effect?
        if (culture.tileInfo.cultures.Count > 1 && Random.value < .1f)
        {
            //Debug.Log("influencing neighbors");
            CultureInfluenceAction influenceNeighbors = new CultureInfluenceAction(culture);
            influenceNeighbors.ExecuteTurn();
        }

    }

    int GrowPopulation()
    {
        if (culture.Population == 1) return 0; // can't reproduce if only one person
        float combinedFertilityRate = culture.FertilityRate * culture.Population;
        return Random.value < combinedFertilityRate ? 1 : 0;
    }

}