using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CultureAggregateInfo : MonoBehaviour
{

    public CultureAggregation cultureAggregate;


    Image cultureColor;
    TextMeshProUGUI name;
    TextMeshProUGUI population;


    public void Init(CultureAggregation ca)
    {
        cultureColor = transform.GetChild(0).GetComponent<Image>();
        name = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        population = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        cultureAggregate = ca;
        EventManager.StartListening("CultureAggregateUpdated" + ca.name, OnCultureAggregateUpdated);
        EventManager.StartListening("CultureAggregateRemoved" + ca.name, OnCultureAggregateRemoved);
        name.text = cultureAggregate.name;
        UpdateInfo();
    }

    void OnCultureAggregateUpdated(Dictionary<string, object> cultureAggregateDict)
    {
        cultureAggregate = (CultureAggregation)cultureAggregateDict["cultureAggregate"];
        UpdateInfo();
    }

    void OnCultureAggregateRemoved(Dictionary<string, object> empty)
    {
        Debug.Log("destroying info panel for " + cultureAggregate.name);
        EventManager.StopListening("CultureAggregateUpdated" + cultureAggregate.name, OnCultureAggregateUpdated);
        EventManager.StopListening("CultureAggregateRemoved" + cultureAggregate.name, OnCultureAggregateRemoved);
        Destroy(gameObject);
    }

    void UpdateInfo()
    {
        cultureColor.color = cultureAggregate.avgColor;
        population.text = "Population: " + cultureAggregate.totalPopulation;
    }

    private void OnDestroy()
    {
        EventManager.StopListening("CultureAggregateUpdated" + cultureAggregate.name, OnCultureAggregateUpdated);
        EventManager.StopListening("CultureAggregateRemoved" + cultureAggregate.name, OnCultureAggregateRemoved);
    }

}
