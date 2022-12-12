using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeWithTileAction : CultureAction
{
    CultureHandler cultureHandler;
    Tile NewTile;

    public MergeWithTileAction(Culture c) : base(c) 
    {
        cultureHandler = Culture.CultureHandler;
        NewTile = cultureHandler.GetComponentInParent<Tile>();
    }

    public override Turn ExecuteTurn()
    {
        return CombineCultureWithNewTile();
    }

    Turn CombineCultureWithNewTile()
    {
        Debug.Log($"merging culture {Culture}({Culture.GetHashCode()}) with tile {NewTile}");
        if (cultureHandler.HasCultureByName(Culture.name)) return AttemptToCombineCultures();
        return AddForeignCulture();
    }

    Turn AttemptToCombineCultures()
    {
        if(Culture.CanMerge(cultureHandler.GetCultureByName(Culture.name)))
        {
            return MergeCultures(cultureHandler.GetCultureByName(Culture.name), Culture);
        }
        return CreateAsNewCulture();
    }

    Turn AddForeignCulture()
    {
        SetExistingCulturesToInvaded();
        cultureHandler.TransferArrivalToTile(Culture);

        return turn;
    }

    void SetExistingCulturesToInvaded()
    {
        foreach (Culture c in cultureHandler.GetAllSettledCultures())
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
