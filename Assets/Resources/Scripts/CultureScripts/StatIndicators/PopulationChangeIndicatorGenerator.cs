using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationChangeIndicatorGenerator : MonoBehaviour
{
    public GameObject PopulationChangeIndicator;

    private void Start()
    {
        GetComponentInParent<Culture>().OnPopulationChanged += PopulationChangeIndicatorGenerator_OnPopulationChanged;
    }

    public void PopulationChangeIndicatorGenerator_OnPopulationChanged(object sender, Culture.OnPopulationChangedEventArgs e)
    {
        CreateIndicator(e.PopChange);
    }
    
    public void CreateIndicator(int popChange)
    {
        GameObject newIndicator = Instantiate(PopulationChangeIndicator, transform);
        string plus = popChange < 0 ? "" : "+";
        newIndicator.GetComponent<TextMeshPro>().text = $"{plus}{popChange}";
        newIndicator.GetComponent<Indicator>().Initialize();
    }


}
