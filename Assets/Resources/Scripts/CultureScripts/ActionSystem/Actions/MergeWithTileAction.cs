using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MergeWithTileAction
{

    public static void CombineCultureWithNewTile(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        Tile newTile = cultureTurnInfo.CurTile;
        CultureHandler cultureHandler = newTile.GetComponentInChildren<CultureHandler>();

        //Debug.Log($"merging culture {Culture}({Culture.GetHashCode()}) with tile {NewTile}");
        if (cultureHandler.HasCultureByName(culture.name))
        {
            AttemptToCombineCultures(cultureTurnInfo, cultureHandler);
            return;
        }
        AddForeignCulture(cultureTurnInfo, cultureHandler);
    }

    static void AttemptToCombineCultures(CultureTurnInfo cultureTurnInfo, CultureHandler cultureHandler)
    {
        Culture culture = cultureTurnInfo.Culture;
        Culture other = cultureHandler.GetCultureByName(culture.name);
        if (culture.CanMerge(other))
        {
            MergeCultures(cultureTurnInfo, other, culture);
            return;
        }
        CreateAsNewCulture(cultureTurnInfo);
    }

    static void AddForeignCulture(CultureTurnInfo cultureTurnInfo, CultureHandler cultureHandler)
    {
        SetExistingCulturesToInvaded(cultureTurnInfo, cultureHandler);
        cultureHandler.TransferArrivalToTile(cultureTurnInfo.Culture);
    }

    static void SetExistingCulturesToInvaded(CultureTurnInfo cultureTurnInfo, CultureHandler cultureHandler)
    {
        Culture culture = cultureTurnInfo.Culture;
        foreach (Culture c in cultureHandler.GetAllSettledCultures())
        {
            Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, c, Culture.State.Invaded));
        }
        Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, culture, Culture.State.Invader));
    }

    static void CreateAsNewCulture(CultureTurnInfo cultureTurnInfo)
    {
        Culture culture = cultureTurnInfo.Culture;
        string newName = Culture.MutateString(culture.Name);
        Turn.AddStringUpdate(CultureUpdateGetter.GetNameUpdate(cultureTurnInfo, culture, newName));
        //Debug.Log($"Changing name of {Culture} to {newName}");
    }

    static void MergeCultures(CultureTurnInfo cultureTurnInfo, Culture remain, Culture merged)
    {
        //Debug.Log("merging cultures");
        float percentThisPopulation = (float)remain.Population / (remain.Population + merged.Population);
        Color lerpedColor = Color.Lerp(remain.Color, merged.Color, percentThisPopulation);

        Turn.AddColorUpdate(CultureUpdateGetter.GetColorUpdate(cultureTurnInfo, remain, lerpedColor));
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, remain, merged.Population));
        Turn.AddIntUpdate(CultureUpdateGetter.GetPopulationUpdate(cultureTurnInfo, merged, -merged.Population));
        Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, merged, Culture.State.PendingRemoval));
        Turn.AddAffinityUpdate(CultureUpdateGetter.GetFullAffinityUpdate(cultureTurnInfo, remain, remain.GetComponent<AffinityManager>().GetStatMerge(merged.GetComponent<AffinityManager>(), percentThisPopulation)));
        Turn.AddStringUpdate(CultureUpdateGetter.GetNameUpdate(cultureTurnInfo, remain, Culture.CombineStrings(remain.Name, merged.name)));
    }
}