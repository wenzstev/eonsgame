using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeAction : CultureAction
{
    public MergeAction(Culture c) : base(c) { }

    public override Turn ExecuteTurn()
    {
        return CombineCultureWithNewTile();
    }



    Turn CombineCultureWithNewTile()
    {
        GameObject newTileObj = culture.transform.parent.gameObject;
        TileInfo newTileInfo = newTileObj.GetComponent<TileInfo>();
        
        turn.UpdateCulture(culture).newTile = newTileObj.GetComponent<Tile>();


        Culture potentialSameCulture = null;
        if (newTileInfo.cultures.TryGetValue(culture.name, out potentialSameCulture))
        {
            if(culture.CanMerge(potentialSameCulture))
            {
                return MergeCultures(potentialSameCulture, culture);
            }

            SetExistingCulturesToInvaded(newTileInfo);
            return CreateAsNewCulture();
            
        }
        else if (newTileInfo.cultures.TryGetValue(culture.GetComponent<CultureMemory>().cultureParentName, out potentialSameCulture))
        {
            if(culture.CanMerge(potentialSameCulture))
            {
                return MergeCultures(culture, potentialSameCulture);
            }       
        }

        SetExistingCulturesToInvaded(newTileInfo);
        return turn;
    }

    void SetExistingCulturesToInvaded(TileInfo newTileInfo)
    {
        foreach (Culture c in newTileInfo.orderToRemoveCulturesIn)
        {
            turn.UpdateCulture(c).newState = Culture.State.Invaded;
        }
        turn.UpdateCulture(culture).newState = Culture.State.Invader;
    }

    public Turn CreateAsNewCulture()
    {

        string newName = Culture.getRandomString(5);
        turn.UpdateCulture(culture).newName = newName;
        return turn;
    }

    public Turn MergeCultures(Culture remain, Culture merged)
    {
        float percentThisPopulation = (float)remain.population / (remain.population + merged.maxPopTransfer);
        turn.UpdateCulture(remain).newColor = Color.Lerp(remain.color, merged.color, percentThisPopulation);
        turn.UpdateCulture(remain).popChange += merged.population;
        turn.UpdateCulture(merged).newState = Culture.State.PendingRemoval;
        return turn;
    }
}
