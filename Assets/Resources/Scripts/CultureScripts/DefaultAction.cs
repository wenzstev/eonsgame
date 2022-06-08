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

        if(culture.tileInfo.cultures.Count > 1)
        {
            // influence
            foreach(Culture c in culture.tileInfo.cultures.Values)
            {
                turn.UpdateCulture(c).newColor = Culture.influenceColor(c, culture);
            }
        }

        turn.UpdateCulture(culture).newState = culture.currentState;

        if (culture.tileInfo.currentMaxPopulation < culture.tileInfo.tilePopulation)
        {
            turn.UpdateCulture(culture).popChange--;
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