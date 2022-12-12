using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureStaging : MonoBehaviour
{
    public List<Culture> NewArrivals {get; private set;}

    private void Start()
    {
        NewArrivals = new List<Culture>();
    }

    public Culture[] GetAllCultures()
    {
        return NewArrivals.ToArray();
    }

    public void AddArrival(Culture c)
    {
        NewArrivals.Add(c);
        c.transform.parent = transform;
    }

    public bool RemoveArrival(Culture c)
    {
        return NewArrivals.Remove(c);
    }
}
