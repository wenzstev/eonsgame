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
        EventManager.StartListening("CultureRemoved", RemoveCulture);
    }

    void UpdateCulture(Dictionary<string, object> cultureToAdd)
    {
        Culture cultureToUpdate = (Culture) cultureToAdd["culture"];

        CultureAggregation potentialCulture = null;
        if (cultures.TryGetValue(cultureToUpdate.name, out potentialCulture))
        {
            potentialCulture.UpdateCultureStats(cultureToUpdate);
            EventManager.TriggerEvent("CultureAggregateUpdated" + potentialCulture.name, new Dictionary<string, object> { { "cultureAggregate", potentialCulture } });
            return;
        }

        CultureAggregation newAggregation = new CultureAggregation(cultureToUpdate);
        //Debug.Log("Adding new culture " + cultureToUpdate.name + "(" + cultureToUpdate.GetHashCode() + ") as aggregation " + newAggregation.GetHashCode());

        cultures.Add(cultureToUpdate.name, newAggregation);
        EventManager.TriggerEvent("CultureAggregateAdded", new Dictionary<string, object> { { "cultureAggregate", newAggregation } });
    }

    void RemoveCulture(Dictionary<string, object> cultureToRemove)
    {
        Culture culture = (Culture) cultureToRemove["culture"];
        CultureAggregation potentialCulture = null;
        if (cultures.TryGetValue(culture.name, out potentialCulture))
        {
            potentialCulture.RemoveCulture(culture);
            if (potentialCulture.totalPopulation == 0)
            {
                //Debug.Log("removing " + culture.name + " from observer as aggregate " + potentialCulture.GetHashCode());
                cultures.Remove(potentialCulture.name);
                EventManager.TriggerEvent("CultureAggregateRemoved"+potentialCulture.name, new Dictionary<string, object> { { "cultureAggregate", potentialCulture } });

            }
            else
            {
                EventManager.TriggerEvent("CultureAggregateUpdated"+potentialCulture.name, new Dictionary<string, object> { { "cultureAggregate", potentialCulture } });
            }

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
