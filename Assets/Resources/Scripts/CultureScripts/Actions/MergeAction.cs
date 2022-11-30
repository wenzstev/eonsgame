using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeAction : CultureAction
{
    CultureContainer cultureContainer;
    GameObject NewTileObj;

    public MergeAction(Culture c) : base(c) 
    {
        cultureContainer = culture.GetComponentInParent<CultureContainer>();
        NewTileObj = culture.GetComponentInParent<Tile>().gameObject;
    }

    public override Turn ExecuteTurn()
    {
        return CombineCultureWithNewTile();
    }

    Turn CombineCultureWithNewTile()
    {
        if (cultureContainer.HasCultureByName(culture)) return AttemptToCombineCultures();
        return AddForeignCulture();
    }

    Turn AttemptToCombineCultures()
    {
        if(culture.CanMerge(cultureContainer.GetCultureByName(culture)))
        {
            return MergeCultures(cultureContainer.GetCultureByName(culture), culture);
        }
        return CreateAsNewCulture();
    }

    Turn AddForeignCulture()
    {
        SetExistingCulturesToInvaded();
        turn.UpdateCulture(culture).newTile = NewTileObj.GetComponent<Tile>();
        return turn;
    }

    void SetExistingCulturesToInvaded()
    {
        foreach (Culture c in cultureContainer.GetAllCultures())
        {
            turn.UpdateCulture(c).newState = Culture.State.Invaded;
        }
        turn.UpdateCulture(culture).newState = Culture.State.Invader;
    }

    public Turn CreateAsNewCulture()
    {

        string newName = Culture.getRandomString(5);
        turn.UpdateCulture(culture).newName = newName;
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
