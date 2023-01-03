using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureStaging : MonoBehaviour
{
    public List<Culture> NewArrivals {get; private set;}

    private void Awake()
    {
        NewArrivals = new List<Culture>();
    }

    public Culture[] GetAllCultures()
    {
        if (NewArrivals.Count != transform.childCount) Debug.LogError("Should never have differnece between children and child count!");
        return NewArrivals.ToArray();
    }

    public void AddArrival(Culture c)
    {
        NewArrivals.Add(c);
        c.OnCultureDestroyed += CultureContainer_OnCultureDestroyed;
        c.transform.parent = transform;
    }

    public bool RemoveArrival(Culture c)
    {
        c.OnCultureDestroyed -= CultureContainer_OnCultureDestroyed;
        return NewArrivals.Remove(c);
    }

    private void CultureContainer_OnCultureDestroyed(object sender, Culture.OnCultureDestroyedEventArgs e)
    {
        NewArrivals.Remove(e.DestroyedCulture);
    }
}
