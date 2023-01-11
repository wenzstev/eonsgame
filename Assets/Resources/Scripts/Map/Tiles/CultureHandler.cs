using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CultureHandler : MonoBehaviour
{
    public CultureContainer CultureContainer;
    public CultureStaging CultureStaging;

    public event EventHandler<OnPopulationChangedEventArgs> OnPopulationChanged;

    private void Awake()
    {
        CultureContainer.OnListChanged += CultureHandler_OnListChanged;
        CultureStaging.OnCulturePopulationChanged += CultureHandler_OnListChanged;
    } 

    public void AddNewArrival(Culture c)
    {
        CultureStaging.AddArrival(c);
        FirePopulationChangedEvent();
    }

    public void BypassArrival(Culture c)
    {
        CultureContainer.AddCulture(c);
        FirePopulationChangedEvent();
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

    public Culture[] GetAllCultures()
    {
        return GetAllSettledCultures().Concat(GetAllStagedCultures()).ToArray();

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
        if (CultureStaging.RemoveArrival(c) || CultureContainer.RemoveCulture(c))
        {
            FirePopulationChangedEvent();
            return;
        }


        Debug.LogError($"Tried to remove culture {c} from tile it wasn't on!");
    }

    
    public void RenameCulture(Culture c, string oldname)
    {
        // rename logic goes here, because we're moving the culture back to staging
        if(CultureContainer.ContainsCulture(c))
        {
            CultureContainer.RemoveCulture(c);
            CultureStaging.AddArrival(c);
        }
    }
    

    void FirePopulationChangedEvent()
    {
        OnPopulationChanged?.Invoke(this, new OnPopulationChangedEventArgs() { CurrentCultures = GetAllCultures() });
    }

    void CultureHandler_OnListChanged(object sender, CultureContainer.OnListChangedEventArgs e)
    {
        FirePopulationChangedEvent();
    }

    public class OnPopulationChangedEventArgs : EventArgs
    {
        public Culture[] CurrentCultures;
    }
}
