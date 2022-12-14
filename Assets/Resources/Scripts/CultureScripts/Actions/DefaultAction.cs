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
        GatherFoodAction gather = new GatherFoodAction(Culture);
        gather.ExecuteTurn();
    }

    void CheckIfHasSufficientFood()
    {
        float overpopulationThreshold = .05f;
        float seekingFoodThreshold = .2f;
        float FoodStore = Culture.GetComponent<CultureFoodStore>().CurrentFoodStore;
        float MaxFoodStore = Culture.GetComponent<CultureFoodStore>().MaxFoodStore;

        Culture.State newState = FoodStore < overpopulationThreshold * MaxFoodStore ? 
            Culture.State.Overpopulated : 
            FoodStore < seekingFoodThreshold * MaxFoodStore ?
            Culture.State.SeekingFood : Culture.State.Default;

        Turn.AddUpdate(new StateUpdate(this, Culture, newState));


    }


    void AddSideEffects()
    {
        Turn.AddUpdate(new PopulationUpdate(this, Culture, GrowPopulation()));

        // need to change influence into a side effect?
        if (Culture.CultureHandler.GetAllSettledCultures().Length > 1 && Random.value < .1f)
        {
            //Debug.Log("influencing neighbors");
            CultureInfluenceAction influenceNeighbors = new CultureInfluenceAction(Culture);
            influenceNeighbors.ExecuteTurn();
        }

        Turn.AddUpdate(new ColorUpdate(this, Culture, Culture.mutateColor(Culture.Color)));

    }

    int GrowPopulation()
    {
        if (Culture.Population == 1) return 0; // can't reproduce if only one person
        float combinedFertilityRate = Culture.FertilityRate * Culture.Population;
        return Random.value < combinedFertilityRate ? 1 : 0;
    }

}