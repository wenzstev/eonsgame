using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeAction : CultureAction
{
    CultureContainer cultureContainer;
    GameObject NewTileObj;

    public MergeAction(Culture c) : base(c) 
    {
        cultureContainer = Culture.Tile.GetComponentInChildren<CultureContainer>();
        NewTileObj = Culture.Tile.gameObject;
    }

    public override Turn ExecuteTurn()
    {
        return CombineCultureWithNewTile();
    }

    Turn CombineCultureWithNewTile()
    {
        Debug.Log($"merging culture {Culture}({Culture.GetHashCode()}) with tile {NewTileObj}");
        if (cultureContainer.HasCultureByName(Culture) && cultureContainer.GetCultureByName(Culture).currentState != Culture.State.NewOnTile) return AttemptToCombineCultures();
        return AddForeignCulture();
    }

    Turn AttemptToCombineCultures()
    {
        if(Culture.CanMerge(cultureContainer.GetCultureByName(Culture)))
        {
            return MergeCultures(cultureContainer.GetCultureByName(Culture), Culture);
        }
        return CreateAsNewCulture();
    }

    Turn AddForeignCulture()
    {
        turn.UpdateCulture(Culture).newTile = NewTileObj.GetComponent<Tile>();
        turn.UpdateCulture(Culture).newState = Culture.State.Default;
        return turn;
    }

    

    void SetExistingCulturesToInvaded()
    {
        foreach (Culture c in cultureContainer.GetAllCultures())
        {
            turn.UpdateCulture(c).newState = Culture.State.Invaded;
        }
        turn.UpdateCulture(Culture).newState = Culture.State.Invader;
    }

    public Turn CreateAsNewCulture()
    {

        string newName = Culture.getRandomString(5);
        turn.UpdateCulture(Culture).newName = newName;
        //Debug.Log("changing culture name (" + culture.GetHashCode() + ") but memory is " + culture.GetComponent<CultureMemory>().previousTile);
        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", newName } });
        SetExistingCulturesToInvaded();
        return turn;
    }

    public Turn MergeCultures(Culture remain, Culture merged)
    {
        //Debug.Log("merging cultures");
        float percentThisPopulation = (float)remain.Population / (remain.Population + merged.Population);
        turn.UpdateCulture(remain).newColor = Color.Lerp(remain.Color, merged.Color, percentThisPopulation);
        turn.UpdateCulture(remain).popChange += merged.Population;
        turn.UpdateCulture(merged).popChange -= merged.Population;
        turn.UpdateCulture(merged).newState = Culture.State.PendingRemoval;
        return turn;
    }
}
