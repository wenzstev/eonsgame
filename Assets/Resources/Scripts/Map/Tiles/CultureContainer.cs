using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CultureContainer : MonoBehaviour
{
    List<Culture> CultureList;
    Dictionary<string, Culture> CultureDictionary;


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        CultureList = new List<Culture>();
    }


    public void AddCulture(Culture culture)
    {
        int newCultureIndex = InsertCultureInList(culture);
        culture.transform.SetParent(transform);
    }

    public void RemoveCulture(Culture culture)
    {
        bool wasRemoved = CultureList.Remove(culture);
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

    int InsertCultureInList(Culture culture)
    {
        for (int i = 0; i < CultureList.Count; i++)
        {
            if (CultureList[i].Population < culture.Population)
            {
                CultureList.Insert(i, culture);
                return i;
            }
        }
        CultureList.Add(culture);
        return 0;
    }
}
