using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CultureStaging : MonoBehaviour
{
    public List<Culture> NewArrivals {get; private set;}

    public event EventHandler<CultureContainer.OnListChangedEventArgs> OnCulturePopulationChanged;

    EventHandler<Culture.OnPopulationChangedEventArgs> cs_OnPopulationChange;
    EventHandler<Culture.OnCultureDestroyedEventArgs> cs_onCultureDestroyed;
    CultureContainer.OnListChangedEventArgs onListChangedEventArgs;
    Dictionary<string, object> cultureDict;

    private void Awake()
    {
        NewArrivals = new List<Culture>();
        cs_OnPopulationChange = CultureContainer_OnCulturePopulationChanged;
        cs_onCultureDestroyed = CultureContainer_OnCultureDestroyed;
        onListChangedEventArgs = new CultureContainer.OnListChangedEventArgs();

    }

    public List<Culture> GetAllCultures()
    {
        //Debug.Log($"Staging is {String.Join(", ", NewArrivals)} on {gameObject.transform.parent.parent}");
        if (NewArrivals.Count != transform.childCount)
        {
            //Debug.Log($"Count of newarrivals is {NewArrivals.Count} and transform is {transform.childCount}");
            Debug.LogError($"{gameObject.transform.parent.parent} has a difference between children and child count!");
        }
        return NewArrivals;
    }

    public void AddArrival(Culture c)
    {
        //Debug.Log($"Adding {c} to staging on {gameObject.transform.parent.parent}.");
        //Debug.Log($"Before, staging is {String.Join(", ", NewArrivals)}");
        NewArrivals.Add(c);
        c.OnCultureDestroyed += cs_onCultureDestroyed;
        c.OnPopulationChanged += cs_OnPopulationChange;
        c.transform.parent = transform;
       // Debug.Log($"Staging is now {String.Join(", ", NewArrivals)} on  {gameObject.transform.parent.parent}");
    }

    public bool RemoveArrival(Culture c)
    {
        //Debug.Log($"Removing {c} from staging on {gameObject.transform.parent.parent}.");
        //Debug.Log($"Before, staging is {String.Join(", ", NewArrivals)}");
        c.OnCultureDestroyed -= cs_onCultureDestroyed;
        c.OnPopulationChanged -= cs_OnPopulationChange;

        bool didRemove = NewArrivals.Remove(c);
        //Debug.Log($"Staging is now {String.Join(", ", NewArrivals)} on  {gameObject.transform.parent.parent}");

        return didRemove;
    }

    private void CultureContainer_OnCultureDestroyed(object sender, Culture.OnCultureDestroyedEventArgs e)
    {
        NewArrivals.Remove(e.DestroyedCulture);
        e.DestroyedCulture.OnCultureDestroyed -= cs_onCultureDestroyed;
        e.DestroyedCulture.OnPopulationChanged -= cs_OnPopulationChange;
    }

    private void CultureContainer_OnCulturePopulationChanged(object sender, Culture.OnPopulationChangedEventArgs e)
    {
        //Debug.Log($"triggered pop change for {sender} on tile {gameObject.transform.parent.parent}");
        onListChangedEventArgs.CultureList = GetAllCultures();
        OnCulturePopulationChanged?.Invoke(this, onListChangedEventArgs);
    }
    

}
