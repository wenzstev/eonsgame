using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public TileDrawer.BiomeType tileType
    {
        get
        {
            return GetComponent<TileDrawer>().tileType;
        }
    }
    public Dictionary<string, Culture> cultures;
    public int popBase;
    public int currentMaxPopulation;
    public int killoffSize = 1;




    public List<Culture> orderToRemoveCulturesIn;


    public int tilePopulation {
        get 
        {
            int pop = 0;
            foreach(Culture c in cultures.Values)
            {
                pop += c.Population;
            }
            return pop;
        } 
    }

    public bool hasTransition = false; // is an animation playing on this tile

    private void Awake()
    {
        cultures = new Dictionary<string, Culture>();
        currentMaxPopulation = popBase;
        orderToRemoveCulturesIn = new List<Culture>();
    }

    public void UpdateMaxOnTile(Culture c)
    {
        if(!cultures.ContainsKey(c.name))
        {
            Debug.LogError("Attempting to update max without culture being on tile!");
            return;
        }
        currentMaxPopulation = Mathf.Max(c.maxOnTile, currentMaxPopulation);

    }
 

    public void Init(int ttype, int popBase)
    {
        //tileType = (TileDrawer.BiomeType) ttype; this will have to be revisited with new saving
        this.popBase = popBase;
    }

    public void UpdateCultureName(string oldname, string newname)
    {
        Culture cultureToChangeName = cultures[oldname];

        cultures.Add(newname, cultureToChangeName);
        cultures.Remove(oldname);
    }

    public void AddCulture(Culture culture)
    {
        cultures.Add(culture.name, culture);    
        orderToRemoveCulturesIn.Add(culture);
        UpdateCultureSurvivability();
        currentMaxPopulation = Mathf.Max(culture.maxOnTile, currentMaxPopulation);
    }

    public void RemoveCulture(Culture culture)
    {
        Culture existingCulture = null;
        if(cultures.TryGetValue(culture.name, out existingCulture))
        {
            if (existingCulture == culture) // removing the entire culture
            {
                cultures.Remove(culture.name);
                orderToRemoveCulturesIn.Remove(culture);
            }
        }

        currentMaxPopulation = popBase;
        foreach(Culture c in cultures.Values)
        {
            currentMaxPopulation = Mathf.Max(c.maxOnTile, currentMaxPopulation);
        }

    }

    public void UpdateCultureSurvivability()
    {
        orderToRemoveCulturesIn.Sort((x, y) => x.maxOnTile.CompareTo(y.maxOnTile));
    }

    public Culture GetRandomCulture()
    {
        return orderToRemoveCulturesIn[orderToRemoveCulturesIn.Count - 1];
    }

    

  
}

