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
        turn.UpdateCulture(culture).newState = Culture.State.Default;

        Culture potentialSameCulture = null;
        if (newTileInfo.cultures.TryGetValue(culture.name, out potentialSameCulture))
        {
            //Debug.Log("culture with same name " + culture.name + " found on tile. attempting to merge");
            if(culture.CanMerge(potentialSameCulture))
            {
                //Debug.Log("close enough to merge. merging.");
                return MergeCultures(potentialSameCulture, culture);
            }

            //Debug.Log("too different. generating new culture");
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
        
        if(newTileInfo.cultures.Count > 0)
        {
            SetExistingCulturesToInvaded(newTileInfo);
        }
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
        Debug.Log("changing culture name (" + culture.GetHashCode() + ") but memory is " + culture.GetComponent<CultureMemory>().previousTile);
        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", newName } });
        return turn;
    }

    public Turn MergeCultures(Culture remain, Culture merged)
    {
        //Debug.Log("merging cultures");
        float percentThisPopulation = (float)remain.population / (remain.population + merged.maxPopTransfer);
        turn.UpdateCulture(remain).newColor = Color.Lerp(remain.color, merged.color, percentThisPopulation);
        turn.UpdateCulture(remain).popChange += merged.population;
        turn.UpdateCulture(merged).popChange -= merged.population;
        turn.UpdateCulture(merged).newState = Culture.State.PendingRemoval;
        return turn;
    }
}
