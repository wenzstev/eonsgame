using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureHandler : MonoBehaviour
{
    public CultureContainer CultureContainer;
    public CultureStaging CultureStaging;

    public void AddNewArrival(Culture c)
    {
        CultureStaging.AddArrival(c);
    }

    public void BypassArrival(Culture c)
    {
        CultureContainer.AddCulture(c);
    }

    public void TransferArrivalToTile(Culture c)
    {
        CultureStaging.RemoveArrival(c);
        CultureContainer.AddCulture(c);
    }

    public Culture[] GetAllSettledCultures()
    {
        return CultureContainer.GetAllCultures();
    }

    public Culture[] GetAllStagedCultures()
    {
        return CultureStaging.GetAllCultures();
    }

    public bool HasCultureByName(string cultureName)
    {
        return CultureContainer.HasCultureByName(cultureName);
    }

    public Culture GetCultureByName(string cultureName)
    {
        return CultureContainer.GetCultureByName(cultureName);
    }

    public void RemoveCulture(Culture c)
    {
        if (CultureStaging.RemoveArrival(c)) return;
        if (CultureContainer.RemoveCulture(c)) return;

        Debug.LogError("Tried to remove culture from tile it wasn't on!");
    }

    public void RenameCulture(Culture c, string oldname)
    {
        if(CultureContainer.ContainsCulture(c)) CultureContainer.ChangeCultureName(c, oldname);
    }
}
