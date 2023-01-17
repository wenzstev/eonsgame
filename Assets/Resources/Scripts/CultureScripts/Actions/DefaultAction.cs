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

        float amountGathered = AttemptToGatherFood();

        // tried to gather food, now perform logic

        CheckIfHasSufficientFood(amountGathered);
        AddSideEffects();

        return turn;
    }

    float AttemptToGatherFood()
    {
        GatherFoodAction gather = new GatherFoodAction(Culture);
        gather.ExecuteTurn();
        return gather.FoodGatheredInTurn;
    }

    void CheckIfHasSufficientFood(float amountGathered)
    {
        if (amountGathered + GetActionCost() > 0) return; // stay in default mode if you're gathering enough food

        float starvingThreshold = .01f;
        float overpopulationThreshold = .05f;
        float seekingFoodThreshold = .2f;
        
        float FoodStore = Culture.GetComponent<CultureFoodStore>().CurrentFoodStore;
        float MaxFoodStore = Culture.GetComponent<CultureFoodStore>().MaxFoodStore;

        Culture.State newState = FoodStore < starvingThreshold * MaxFoodStore ? Culture.State.Starving :
            FoodStore < overpopulationThreshold * MaxFoodStore ? 
            Culture.State.Overpopulated : 
            FoodStore < seekingFoodThreshold * MaxFoodStore ?
            Culture.State.SeekingFood : Culture.State.Default;

        

        Turn.AddUpdate(CultureUpdateGetter.GetStateUpdate(this, Culture, newState));


    }


    void AddSideEffects()
    {
        Turn.AddUpdate(CultureUpdateGetter.GetPopulationUpdate(this, Culture, GrowPopulation()));

        // need to change influence into a side effect?
        if (Culture.CultureHandler.GetAllSettledCultures().Length > 1 && Random.value < .1f)
        {
            //Debug.Log("influencing neighbors");
            CultureInfluenceAction influenceNeighbors = new CultureInfluenceAction(Culture);
            influenceNeighbors.ExecuteTurn();
        }

        Turn.AddUpdate(CultureUpdateGetter.GetColorUpdate(this, Culture, Culture.mutateColor(Culture.Color)));

    }

    int GrowPopulation()
    {
        if (Culture.Population == 1) return 0; // can't reproduce if only one person
        float combinedFertilityRate = Culture.FertilityRate * Culture.Population;
        return Random.value < combinedFertilityRate ? 1 : 0;
    }

}