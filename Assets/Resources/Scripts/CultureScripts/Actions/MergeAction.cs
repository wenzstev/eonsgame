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
        Turn.AddUpdate(new TileUpdate(this, Culture, NewTileObj.GetComponent<Tile>()));
        Turn.AddUpdate(new StateUpdate(this, Culture, Culture.State.Default));
        return turn;
    }

    void SetExistingCulturesToInvaded()
    {
        foreach (Culture c in cultureContainer.GetAllCultures())
        {
            Turn.AddUpdate(new StateUpdate(this, c, Culture.State.Invaded));
        }
        Turn.AddUpdate(new StateUpdate(this, Culture, Culture.State.Invader));
    }

    public Turn CreateAsNewCulture()
    {

        string newName = Culture.getRandomString(5);
        Turn.AddUpdate(new NameUpdate(this, Culture, newName));
        //Debug.Log("changing culture name (" + culture.GetHashCode() + ") but memory is " + culture.GetComponent<CultureMemory>().previousTile);
        EventManager.TriggerEvent("CultureCreated", new Dictionary<string, object> { { "culture", newName } });
        SetExistingCulturesToInvaded();
        return turn;
    }

    public Turn MergeCultures(Culture remain, Culture merged)
    {
        //Debug.Log("merging cultures");
        float percentThisPopulation = (float)remain.Population / (remain.Population + merged.Population);
        Color lerpedColor = Color.Lerp(remain.Color, merged.Color, percentThisPopulation);

        Turn.AddUpdate(new ColorUpdate(this, remain, lerpedColor));
        Turn.AddUpdate(new PopulationUpdate(this, remain, merged.Population));
        Turn.AddUpdate(new PopulationUpdate(this, merged, -merged.Population));
        Turn.AddUpdate(new StateUpdate(this, merged, Culture.State.PendingRemoval)); ;


        return turn;
    }
}
