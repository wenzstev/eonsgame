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
        c.transform.SetParent(CultureStaging.transform, true);
        CultureStaging.AddArrival(c);
        FirePopulationChangedEvent();
    }

    public void BypassArrival(Culture c)
    {
        c.transform.SetParent(CultureContainer.transform, true);
        CultureContainer.AddCulture(c);
        FirePopulationChangedEvent();
    }

    public void TransferArrivalToTile(Culture c)
    {
        //Debug.Log($"Transferring {c} to settled on {transform.parent}");
        CultureStaging.RemoveArrival(c);
        c.transform.SetParent(CultureContainer.transform, true);
        CultureContainer.AddCulture(c);
    }

    public List<Culture> GetAllSettledCultures()
    {
        return CultureContainer.GetAllCultures();
    }

    public List<Culture> GetAllStagedCultures()
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
            c.transform.SetParent(null, true);
            FirePopulationChangedEvent();
            return;
        }


        Debug.LogError($"Tried to remove culture {c} from tile it wasn't on ({transform.parent})!");
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
