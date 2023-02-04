using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MergeWithTileAction
{

    public static void CombineCultureWithNewTile(CultureTurnInfo cultureTurnInfo)
    {
        //Debug.Log($"Adding {cultureTurnInfo.Culture} to tile {cultureTurnInfo.CurTile}");
        Culture culture = cultureTurnInfo.Culture;
        Tile newTile = cultureTurnInfo.CurTile;
        CultureHandler cultureHandler = newTile.GetComponentInChildren<CultureHandler>();

        List<Culture> CultureList = cultureHandler.GetAllSettledCultures();

        foreach (Culture c in CultureList)
        {
            if (AttemptToCombineCultures(culture, c, cultureTurnInfo)) return;
        }
        AddForeignCulture(cultureTurnInfo, cultureHandler);

    }

    static bool AttemptToCombineCultures(Culture merging, Culture onTile, CultureTurnInfo cultureTurnInfo)
    {
        if(merging.CanMerge(onTile)) // culture will attempt to prevent others if overpopulated
        {
            //Debug.Log($"Can combine {merging} with {onTile}");
            MergeCultures(cultureTurnInfo, onTile, merging);
            return true;
        }
        return false;
    }

    static void AddForeignCulture(CultureTurnInfo cultureTurnInfo, CultureHandler cultureHandler)
    {
        //Debug.Log($"Adding {cultureTurnInfo.Culture} as foreign culture");
        SetExistingCulturesToInvaded(cultureTurnInfo, cultureHandler);
        cultureHandler.TransferArrivalToTile(cultureTurnInfo.Culture);
    }

    static void SetExistingCulturesToInvaded(CultureTurnInfo cultureTurnInfo, CultureHandler cultureHandler)
    {
        Culture culture = cultureTurnInfo.Culture;
        List<Culture> ExistingCultures = cultureHandler.GetAllSettledCultures();
        if (ExistingCultures.Count < 1)
        {
            Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, culture, Culture.State.Default));
            return; // no cultures on tile, not an invader
        }

        foreach (Culture c in ExistingCultures)
        {
            Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, c, Culture.State.Invaded));
        }
        Turn.AddStateUpdate(CultureUpdateGetter.GetStateUpdate(cultureTurnInfo, culture, Culture.State.Invader));
    }


    static void MergeCultures(CultureTurnInfo cultureTurnInfo, Culture remain, Culture merged)
    {
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
