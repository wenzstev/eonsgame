using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CultureContainer : MonoBehaviour
{
    List<Culture> CultureList;
    Dictionary<string, Culture> CultureDictionary;

    public event EventHandler<OnListChangedEventArgs> OnListChanged;


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        CultureList = new List<Culture>();
        CultureDictionary = new Dictionary<string, Culture>();
    }

    public void AddCulture(Culture culture)
    {
        if (!HasCultureByName(culture))
        {
            InsertCultureInList(culture);
            culture.transform.SetParent(transform);
            InsertCultureInDictionary(culture);
        }

        culture.OnPopulationChanged += CultureContainer_OnPopulationChanged;
        SortListByPopulation();
    }

    public void RemoveCulture(Culture culture)
    {
        bool wasRemoved = CultureList.Remove(culture);
        culture.OnPopulationChanged -= CultureContainer_OnPopulationChanged;
        if (!wasRemoved) Debug.LogError("Tried to remove culture from tile that wasn't in the CulturePlacementHandler list!");
    }

    public Culture[] GetAllCultures()
    {
        return CultureList.ToArray();
    }

    public bool HasCultureByName(Culture culture)
    {
        return CultureDictionary.ContainsKey(culture.name);
    }

    public Culture GetCultureByName(Culture culture)
    {
        return CultureDictionary[culture.name];
    }

    void InsertCultureInList(Culture culture) 
    {
        CultureList.Add(culture);
    }

    void SortListByPopulation()
    {
        CultureList = CultureList.OrderByDescending(c => c.Population).ToList();
        OnListChanged?.Invoke(this, new OnListChangedEventArgs() { CultureList = GetAllCultures() });
    }



    void InsertCultureInDictionary(Culture culture)
    {
        CultureDictionary.Add(culture.name, culture);
    }

    public class OnListChangedEventArgs : EventArgs
    {
        public Culture[] CultureList;
    }

    private void CultureContainer_OnPopulationChanged(object sender, Culture.OnPopulationChangedEventArgs e)
    {
        SortListByPopulation();
    }
}
