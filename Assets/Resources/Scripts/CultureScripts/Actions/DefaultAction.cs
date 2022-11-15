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

        GatherFoodAction AttemptToGatherFood = new GatherFoodAction(culture);
        AttemptToGatherFood.ExecuteTurn();

        // tried to gather food, now perform logic

        CheckIfHasSufficientFood();
        AddSideEffects();

        return turn;
    }

    void CheckIfHasSufficientFood()
    {
        float FoodChange = turn.turnUpdates[culture].FoodChange;
        float FoodStore = culture.GetComponent<CultureFoodStore>().CurrentFoodStore;
        turn.UpdateCulture(culture).newState = FoodStore + FoodChange < culture.Population ? Culture.State.Overpopulated : FoodChange < culture.Population ? Culture.State.SeekingFood : Culture.State.Default;
    }



    void AddSideEffects()
    {
        if (Random.value < culture.growPopulationChance)
        {
            turn.UpdateCulture(culture).popChange++;
        }

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

}