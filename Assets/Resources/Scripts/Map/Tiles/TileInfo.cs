using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileInfo : MonoBehaviour
{

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

    }


    

  
}

