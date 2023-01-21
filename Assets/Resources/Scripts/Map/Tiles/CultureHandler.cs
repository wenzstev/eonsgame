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
<<<<<<< HEAD
        c.transform.SetParent(CultureStaging.transform, true);
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        CultureStaging.AddArrival(c);
        FirePopulationChangedEvent();
    }

    public void BypassArrival(Culture c)
    {
<<<<<<< HEAD
        c.transform.SetParent(CultureContainer.transform, true);
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
        CultureContainer.AddCulture(c);
        FirePopulationChangedEvent();
    }

    public void TransferArrivalToTile(Culture c)
    {
<<<<<<< HEAD
        //Debug.Log($"Transferring {c} to settled on {transform.parent}");
        CultureStaging.RemoveArrival(c);
        c.transform.SetParent(CultureContainer.transform, true);
=======
        CultureStaging.RemoveArrival(c);
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
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
<<<<<<< HEAD
            c.transform.SetParent(null, true);
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
            FirePopulationChangedEvent();
            return;
        }


<<<<<<< HEAD
        Debug.LogError($"Tried to remove culture {c} from tile it wasn't on ({transform.parent})!");
=======
        Debug.LogError($"Tried to remove culture {c} from tile it wasn't on!");
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }

    
    public void RenameCulture(Culture c, string oldname)
    {
        // rename logic goes here, because we're moving the culture back to staging
        if(CultureContainer.ContainsCulture(c))
        {
            CultureContainer.RemoveCulture(c);
            CultureStaging.AddArrival(c);
<<<<<<< HEAD
            c.transform.parent = CultureStaging.transform;
=======
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
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
