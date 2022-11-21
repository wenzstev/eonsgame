using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationChangeIndicatorGenerator : MonoBehaviour
{
    public GameObject PopulationChangeIndicator;

    
    public void CreateIndicator(int popChange)
    {
        GameObject newIndicator = Instantiate(PopulationChangeIndicator, transform);
        string plus = popChange < 0 ? "" : "+";
        newIndicator.GetComponent<TextMeshPro>().text = $"{plus}{popChange}";
        newIndicator.GetComponent<Indicator>().Initialize();
    }


}
