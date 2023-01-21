using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CultureStaging : MonoBehaviour
{
    public List<Culture> NewArrivals {get; private set;}

    public event EventHandler<CultureContainer.OnListChangedEventArgs> OnCulturePopulationChanged;

    private void Awake()
    {
        NewArrivals = new List<Culture>();
    }

    public Culture[] GetAllCultures()
    {
        //Debug.Log($"Staging is {String.Join(", ", NewArrivals)} on {gameObject.transform.parent.parent}");
        if (NewArrivals.Count != transform.childCount)
        {
            //Debug.Log($"Count of newarrivals is {NewArrivals.Count} and transform is {transform.childCount}");
            Debug.LogError($"{gameObject.transform.parent.parent} has a difference between children and child count!");
        }
        return NewArrivals.ToArray();
    }

    public void AddArrival(Culture c)
    {
        //Debug.Log($"Adding {c} to staging on {gameObject.transform.parent.parent}.");
        //Debug.Log($"Before, staging is {String.Join(", ", NewArrivals)}");
        NewArrivals.Add(c);
        c.OnCultureDestroyed += CultureContainer_OnCultureDestroyed;
        c.OnPopulationChanged += CultureContainer_OnCulturePopulationChanged;
        c.transform.parent = transform;
       // Debug.Log($"Staging is now {String.Join(", ", NewArrivals)} on  {gameObject.transform.parent.parent}");
    }

    public bool RemoveArrival(Culture c)
    {
        //Debug.Log($"Removing {c} from staging on {gameObject.transform.parent.parent}.");
        //Debug.Log($"Before, staging is {String.Join(", ", NewArrivals)}");
        c.OnCultureDestroyed -= CultureContainer_OnCultureDestroyed;
        c.OnPopulationChanged -= CultureContainer_OnCulturePopulationChanged;

        bool didRemove = NewArrivals.Remove(c);
        //Debug.Log($"Staging is now {String.Join(", ", NewArrivals)} on  {gameObject.transform.parent.parent}");

        return didRemove;
    }

    private void CultureContainer_OnCultureDestroyed(object sender, Culture.OnCultureDestroyedEventArgs e)
    {
        NewArrivals.Remove(e.DestroyedCulture);
        e.DestroyedCulture.OnCultureDestroyed -= CultureContainer_OnCultureDestroyed;
        e.DestroyedCulture.OnPopulationChanged -= CultureContainer_OnCulturePopulationChanged;
    }

    private void CultureContainer_OnCulturePopulationChanged(object sender, Culture.OnPopulationChangedEventArgs e)
    {
        //Debug.Log($"triggered pop change for {sender} on tile {gameObject.transform.parent.parent}");
        OnCulturePopulationChanged?.Invoke(this, new CultureContainer.OnListChangedEventArgs() { CultureList = GetAllCultures() });
    }
    

}
