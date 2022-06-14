using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAction : CultureAction
{
    public DefaultAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {

        if (Random.value < culture.spreadChance)
        {
            CultureAction moveTile = new MoveTileAction(culture);
            return moveTile.ExecuteTurn();
        }

        if(culture.tileInfo.cultures.Count > 1 && Random.value < .1f)
        {
            CultureInfluenceAction influenceNeighbors = new CultureInfluenceAction(culture);
            return influenceNeighbors.ExecuteTurn();

            // how do I want merging/influence to work?
            // - let's create a cultureinfluenceaction
            // - each culture has a small influence on each other culture on the tile
            // - influence level is determined by culture's population and (later) it's tech level
            // - if two cultures are close enough in color, they can merge, creating new culture
            



        }

        turn.UpdateCulture(culture).newState = culture.currentState;

        if (culture.tileInfo.currentMaxPopulation < culture.tileInfo.tilePopulation)
        {
            turn.UpdateCulture(culture).popChange--;
            //Debug.Log(culture.tileInfo.name + " " + culture.GetHashCode() + " " + culture.population);
            //EventManager.TriggerEvent("PauseSpeed", null);
        }


        if (Random.value < culture.growPopulationChance)
        {
            turn.UpdateCulture(culture).popChange++;
        }

        if (Random.value < culture.gainAffinityChance)
        {
            turn.UpdateCulture(culture).newAffinity = culture.tileInfo.tileType;
        }

        return turn;


    }
}