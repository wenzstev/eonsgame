using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureObserver : MonoBehaviour
{
    Dictionary<string, CultureAggregation> cultures;

 

    private void Start()
    {
        cultures = new Dictionary<string, CultureAggregation>();
        EventManager.StartListening("CultureUpdated", UpdateCulture);
        EventManager.StartListening("CultureAggregateRemoved", RemoveAggregate);
        EventManager.StartListening("CultureRemoved", RemoveCulture);
    }

    void UpdateCulture(Dictionary<string, object> cultureToAdd)
    {
        Culture cultureToUpdate = (Culture) cultureToAdd["culture"];
        Debug.Log("updating " + cultureToUpdate.GetHashCode());

        CultureAggregation potentialCulture = null;
        if (cultures.TryGetValue(cultureToUpdate.name, out potentialCulture))
        {
            potentialCulture.UpdateCultureStats(cultureToUpdate);
            Debug.Log("triggering on " + potentialCulture.name);
            return;
        }

        CultureAggregation newAggregation = new CultureAggregation(cultureToUpdate);
        //Debug.Log("Adding new culture " + cultureToUpdate.name + "(" + cultureToUpdate.GetHashCode() + ") as aggregation " + newAggregation.GetHashCode());

        cultures.Add(cultureToUpdate.name, newAggregation);
        EventManager.TriggerEvent("CultureAggregateAdded", new Dictionary<string, object> { { "cultureAggregate", newAggregation } });
    }

    void RemoveAggregate(Dictionary<string, object> aggregateToRemove)
    {
        CultureAggregation aggregate = (CultureAggregation)aggregateToRemove["cultureAggregate"];
        cultures.Remove(aggregate.name);

    }

    void RemoveCulture(Dictionary<string, object> cultureToRemove)
    {
        Culture culture = (Culture) cultureToRemove["culture"];
        CultureAggregation potentialCulture = null;
        if (cultures.TryGetValue(culture.name, out potentialCulture))
        {
            potentialCulture.RemoveCulture(culture);
            return;
        }

        Debug.LogError("attempted to remove a culture (" + culture.name + ") that doesn't exist!");
    }




}

public class CultureAggregation
{
    HashSet<Culture> cultures;
    public int totalPopulation;
    public Color avgColor;
    public string name;
    float cultureDistanceThreshold = .1f;

    public CultureAggregation(Culture culture)
    {
        cultures = new HashSet<Culture>();
        cultures.Add(culture);
        avgColor = culture.color;
        name = culture.name;
        totalPopulation = culture.population;

    }

    public void RemoveCulture(Culture cultureToRemove)
    {
        cultures.Remove(cultureToRemove);
        RecaulculateStats();

    }

    public void UpdateCultureStats(Culture cultureToAdd)
    {

        
        AddCulture(cultureToAdd);
        if(cultureToAdd.population == 0)
        {
            cultures.Remove(cultureToAdd);
        }

        RecaulculateStats();

    }

    void RecaulculateStats()
    {
        totalPopulation = 0;
        float r = 0;
        float g = 0;
        float b = 0;

        foreach (Culture c in cultures)
        {
            //Debug.Log(c.name + "(" + c.GetHashCode() + ") has pop of " + c.population);
            totalPopulation += c.population;
            r += c.color.r;
            g += c.color.g;
            b += c.color.b;
        }

        int numCultures = cultures.Count;

        avgColor = new Color(r / numCultures, g / numCultures, b / numCultures);
        if(totalPopulation == 0)
        {
            Debug.Log("pop of " + name + "  is zero. destroying");
            EventManager.TriggerEvent("CultureAggregateRemoved", new Dictionary<string, object> { { "cultureAggregate", this } });
            EventManager.TriggerEvent("CultureAggregateRemoved" + name, new Dictionary<string, object> { { "cultureAggregate", this } });
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


}
