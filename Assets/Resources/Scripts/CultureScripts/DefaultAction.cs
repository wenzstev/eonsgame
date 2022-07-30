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
            //Debug.Log("influencing neighbors");
            CultureInfluenceAction influenceNeighbors = new CultureInfluenceAction(culture);
            return influenceNeighbors.ExecuteTurn();
        }

        turn.UpdateCulture(culture).newState = culture.currentState;


        if (culture.tileInfo.currentMaxPopulation < culture.tileInfo.tilePopulation)
        {
            turn.UpdateCulture(culture).newState = Culture.State.Overpopulated;
            return turn;
        }


        if (Random.value < culture.growPopulationChance)
        {
            turn.UpdateCulture(culture).popChange++;
        }

        if (Random.value < culture.gainAffinityChance)
        {
            turn.UpdateCulture(culture).newAffinity = (int) culture.tileInfo.tileType;
        }

        return turn;


    }
}