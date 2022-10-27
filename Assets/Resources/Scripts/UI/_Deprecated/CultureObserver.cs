using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureObserver : MonoBehaviour
{
    Dictionary<string, CultureAggregation> cultures;

 
    private void Awake()
    {
        //Debug.Log("starting observer");
        cultures = new Dictionary<string, CultureAggregation>();

        EventManager.StartListening("CultureCreated", CreateCulture);
        EventManager.StartListening("CultureAggregateRemoved", RemoveAggregate);
    }

    void CreateCulture(Dictionary<string, object> newCultureDict)
    {
        string newCulture = (string)newCultureDict["culture"];
        if (cultures.ContainsKey(newCulture))
        {
            Debug.LogError("Attempting to create a culture (" + newCulture + ") that already exists!");
            return;
        }
        CultureAggregation newAggregation = new CultureAggregation(newCulture);
    }

    void RemoveAggregate(Dictionary<string, object> aggregateToRemove)
    {
        CultureAggregation aggregate = (CultureAggregation)aggregateToRemove["cultureAggregate"];
        cultures.Remove(aggregate.name);

    }

    private void OnDestroy()
    {
        EventManager.StopListening("CultureCreated", CreateCulture);
        EventManager.StopListening("CultureAggregateRemoved", RemoveAggregate);
        foreach(CultureAggregation ca in cultures.Values)
        {
            ca.RemoveAggregate();
        }
    }
}

public class CultureAggregation
{
    HashSet<Culture> cultures;
    public int totalPopulation;
    public Color avgColor;
    public string name;

    public CultureAggregation(string newCultureName)
    {
        name = newCultureName;
        cultures = new HashSet<Culture>();

        EventManager.StartListening("CultureUpdated" + name, UpdateCultureStats);
        EventManager.StartListening("CultureRemoved" + name, RemoveCulture);
        EventManager.TriggerEvent("CultureAggregateAdded", new Dictionary<string, object> { { "cultureAggregate", this } });
    }

    public CultureAggregation(Culture culture)
    {
        cultures = new HashSet<Culture>();
        cultures.Add(culture);
        avgColor = culture.Color;
        name = culture.name;
        totalPopulation = culture.Population;

        EventManager.StartListening("CultureUpdated" + name, UpdateCultureStats);
        EventManager.StartListening("CultureRemoved" + name, RemoveCulture);
        EventManager.TriggerEvent("CultureAggregateAdded", new Dictionary<string, object> { { "cultureAggregate", this } });


    }

    public void RemoveCulture(Dictionary<string, object> removedCultureDict)
    {
        Culture cultureToRemove = (Culture)removedCultureDict["culture"];
        cultures.Remove(cultureToRemove);
        RecaulculateStats();
    }

    public void UpdateCultureStats(Dictionary<string, object> updatedCultureDict)
    {
        Culture updatedCulture = (Culture)updatedCultureDict["culture"];

        AddCulture(updatedCulture);
        if(updatedCulture.Population == 0)
        {
            cultures.Remove(updatedCulture);
        }
        RecaulculateStats();
    }

    void RecaulculateStats()
    {
        //Debug.Log("name is " + name);
        totalPopulation = 0;
        float r = 0;
        float g = 0;
        float b = 0;

        foreach (Culture c in cultures)
        {
            //Debug.Log(c.name + "(" + c.GetHashCode() + ") has pop of " + c.population);
            totalPopulation += c.Population;
            r += c.Color.r;
            g += c.Color.g;
            b += c.Color.b;
        }

        int numCultures = cultures.Count;

        avgColor = new Color(r / numCultures, g / numCultures, b / numCultures);
        if(totalPopulation == 0)
        {
            //Debug.Log("pop of " + name + "  is zero. destroying aggregate");
            RemoveAggregate();
        }
        else
        {
            EventManager.TriggerEvent("CultureAggregateUpdated" + name, new Dictionary<string, object> { { "cultureAggregate", this } });

        }

    }

    bool HasCulture(Culture culture)
    {
        return cultures.Contains(culture);
    }

    void AddCulture(Culture culture)
    {
        cultures.Add(culture);
    }

    public void RemoveAggregate()
    {
        EventManager.TriggerEvent("CultureAggregateRemoved", new Dictionary<string, object> { { "cultureAggregate", this } });
        EventManager.TriggerEvent("CultureAggregateRemoved" + name, new Dictionary<string, object> { { "cultureAggregate", this } });
        EventManager.StopListening("CultureUpdated" + name, UpdateCultureStats);
        EventManager.StopListening("CultureRemoved" + name, RemoveCulture);
    }


}
