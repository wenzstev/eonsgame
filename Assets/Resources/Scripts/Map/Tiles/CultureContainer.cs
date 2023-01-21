using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CultureContainer : MonoBehaviour
{
    List<Culture> CultureList;

    public event EventHandler<OnListChangedEventArgs> OnListChanged;


    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        CultureList = new List<Culture>();
    }

    public void AddCulture(Culture culture)
    {
        //Debug.Log($"Before: {PrintContents()}");
        InsertCultureInList(culture);
        culture.OnPopulationChanged += CultureContainer_OnPopulationChanged;
        culture.OnCultureDestroyed += CultureContainer_OnCultureDestroyed;
        SortListByPopulation();
       // Debug.Log($"After: {PrintContents()}");

    }

    public bool RemoveCulture(Culture culture)
    {
        //Debug.Log($"Before: {PrintContents()}");

        bool wasRemoved = CultureList.Remove(culture);
        if (!wasRemoved) return false;

        //Debug.Log($"After: {PrintContents()}");


        SortListByPopulation();
        culture.OnPopulationChanged -= CultureContainer_OnPopulationChanged;
        culture.OnCultureDestroyed -= CultureContainer_OnCultureDestroyed;



        return true;
    }

    public Culture[] GetAllCultures()
    {
        return CultureList.ToArray();
    }

    
    public bool HasCultureByName(string cultureName)
    {
        return CultureList.Select(c => c.Name == cultureName).Any(b => b);
    }
    

    
    public Culture GetCultureByName(string cultureName)
    {
        return CultureList.Where(c => c.Name == cultureName).FirstOrDefault();
    }
    

    public bool ContainsCulture(Culture c)
    {
        return CultureList.Contains(c);
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

    public class OnListChangedEventArgs : EventArgs
    {
        public Culture[] CultureList;
    }

    private void CultureContainer_OnPopulationChanged(object sender, Culture.OnPopulationChangedEventArgs e)
    {
        SortListByPopulation();
    }

    private void CultureContainer_OnCultureDestroyed(object sender, Culture.OnCultureDestroyedEventArgs e)
    {
        RemoveCulture(e.DestroyedCulture);
        SortListByPopulation();
    }

    public string PrintContents()
    {
        string list = String.Join(',', CultureList.Select(c => c.ToString()));

        return $"ListVals: {list}";
    }
}
