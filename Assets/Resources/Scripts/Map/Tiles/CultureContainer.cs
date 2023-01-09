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


    private void Awake()
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
        Debug.Log($"Adding culture {culture} to tile {gameObject.transform.parent.parent}");
        Debug.Log($"Before: {PrintContents()}");

        if (!HasCultureByName(culture.Name))
        {
            InsertCultureInList(culture);
            culture.transform.SetParent(transform);
            InsertCultureInDictionary(culture);
        }

        culture.OnPopulationChanged += CultureContainer_OnPopulationChanged;
        culture.OnCultureDestroyed += CultureContainer_OnCultureDestroyed;
        SortListByPopulation();
        culture.transform.parent = transform;
        Debug.Log($"After: {PrintContents()}");

    }

    public bool RemoveCulture(Culture culture)
    {
        Debug.Log($"Removing culture {culture} from tile {gameObject.transform.parent.parent}");
        Debug.Log($"Before: {PrintContents()}");

        bool wasRemoved = CultureList.Remove(culture);
        if (!wasRemoved) return false;

        CultureDictionary.Remove(culture.name); // <-- this doesn't work right now because the culture, when removed, may have a different name

        Debug.Log($"After: {PrintContents()}");


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
         return CultureDictionary.ContainsKey(cultureName);
    }
    

    
    public Culture GetCultureByName(string cultureName)
    {
        Culture c;
        if (CultureDictionary.TryGetValue(cultureName, out c)) return c;
        return null;
    }
    

    public bool ContainsCulture(Culture c)
    {
        return CultureList.Contains(c);
    }

    
    public void ChangeCultureName(Culture culture, string oldName)
    {
        Debug.Log($"Renaming culture {culture} from old name {oldName}");
        Culture c = GetCultureByName(oldName);
        if (c != culture) { Debug.LogError("trying to change culture name that doesn't exist on tile."); return; }

        Culture potentialNamed = GetCultureByName(culture.Name);
        if(potentialNamed)
        {
            Debug.Log(potentialNamed);
            
        }


        CultureDictionary.Remove(oldName);
        CultureDictionary.Add(culture.name, culture);
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

    private void CultureContainer_OnCultureDestroyed(object sender, Culture.OnCultureDestroyedEventArgs e)
    {
        RemoveCulture(e.DestroyedCulture);
        SortListByPopulation();
    }

    public string PrintContents()
    {
        string list = String.Join(',', CultureList.Select(c => c.ToString()));
        string dict = String.Join(',', CultureDictionary.Select(v => $"{v.Key}: {v.Value}"));

        return $"ListVals: {list} DictVals: {dict}";
    }
}
