using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Profiling;

public class CultureContainer : MonoBehaviour
{
    List<Culture> CultureList;

    public event EventHandler<OnListChangedEventArgs> OnListChanged;

    delegate void AddListHandler(object sender, Culture.OnPopulationChangedEventArgs e);

    EventHandler<Culture.OnPopulationChangedEventArgs> cc_OnPopulationChange;
    EventHandler<Culture.OnCultureDestroyedEventArgs> cc_onCulutureDestroyed;

    OnListChangedEventArgs onListChangedEventArgs;

    private void Awake()
    {
        Initialize();
    }

    AddListHandler handler;

    public void Initialize()
    {
        CultureList = new List<Culture>();

        // create the eventhandlers and cache them to prevent boxing later
        cc_OnPopulationChange = CultureContainer_OnPopulationChanged;
        cc_onCulutureDestroyed = CultureContainer_OnCultureDestroyed;
        onListChangedEventArgs = new OnListChangedEventArgs() { CultureList = CultureList };
    }

    public void AddCulture(Culture culture)
    {
        //Debug.Log($"Before: {PrintContents()}");
        InsertCultureIntoList(culture);
        culture.OnPopulationChanged += cc_OnPopulationChange;
        culture.OnCultureDestroyed += cc_onCulutureDestroyed;

       // Debug.Log($"After: {PrintContents()}");

    }

    public bool RemoveCulture(Culture culture)
    {
        //Debug.Log($"Before: {PrintContents()}");

        bool wasRemoved = CultureList.Remove(culture);
        if (!wasRemoved) return false;

        //Debug.Log($"After: {PrintContents()}");

        culture.OnPopulationChanged -= cc_OnPopulationChange;
        culture.OnCultureDestroyed -= cc_onCulutureDestroyed;
        InvokeListChange();

        return true;
    }

    public List<Culture> GetAllCultures()
    {
        return CultureList;
    }

    
    public bool HasCultureByName(string cultureName)
    {
        for(int i = 0; i < CultureList.Count; i++)
        {
            if (cultureName == CultureList[i].Name) return true;
        }
        return false;
    }
    

    
    public Culture GetCultureByName(string cultureName)
    {
        for (int i = 0; i < CultureList.Count; i++)
        {
            if (cultureName == CultureList[i].Name) return CultureList[i];
        }
        return null;
    }
    

    public bool ContainsCulture(Culture c)
    {
        return CultureList.Contains(c);
    }


    void InsertCultureIntoList(Culture c)
    {
        CultureList.Remove(c); // if culture is already present, remove
        int curCultureIndex = 0;
        while (curCultureIndex < CultureList.Count && c.Population < CultureList[curCultureIndex].Population) curCultureIndex++;

        CultureList.Insert(curCultureIndex, c);
        InvokeListChange();
    }

    void InvokeListChange()
    {
        OnListChanged?.Invoke(this, onListChangedEventArgs);
    }

    public class OnListChangedEventArgs : EventArgs
    {
        public List<Culture> CultureList;
    }

    void CultureContainer_OnPopulationChanged(object sender, Culture.OnPopulationChangedEventArgs e)
    {
        InsertCultureIntoList(e.Culture);
    }

     void CultureContainer_OnCultureDestroyed(object sender, Culture.OnCultureDestroyedEventArgs e)
    {
        RemoveCulture(e.DestroyedCulture);
    }

    public string PrintContents()
    {
        string list = String.Join(',', CultureList.Select(c => c.ToString()));

        return $"ListVals: {list}";
    }
}
