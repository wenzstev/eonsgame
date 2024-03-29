using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CulturePanelController: MonoBehaviour
{
    public TextMeshProUGUI CultureName;
    public TextMeshProUGUI CulturePop;
    public Image CultureColor;
    public AffinityDisplay AffinityDisplay;


    Culture Culture;

    public void SetValues(GameObject c)
    {
        Culture = c.GetComponent<Culture>();

        SetNameText(Culture.name);
        SetPopulationText(Culture.Population);
        SetColor(Culture.Color);

        AffinityDisplay.SetValues(Culture);
        GetComponentInChildren<ZoomToObj>().SetSelectedObj(c);

        Culture.OnPopulationChanged += Culture_OnPopulationChanged;
        Culture.OnNameChanged += Culture_OnNameChanged;
        Culture.OnColorChanged += Culture_OnColorChanged;
        Culture.OnCultureDestroyed += Culture_OnDestroyed;
    }

    private void Culture_OnColorChanged(object sender, Culture.OnColorChangedEventArgs e)
    {
        SetColor(e.color);
    }

    private void Culture_OnNameChanged(object sender, Culture.OnCultureNameChangedEventArgs e)
    {
        SetNameText(e.NewName);
    }

    private void Culture_OnPopulationChanged(object sender, Culture.OnPopulationChangedEventArgs e)
    {
        SetPopulationText(Culture.Population);
    }

    private void Culture_OnDestroyed(object sender, Culture.OnCultureDestroyedEventArgs e)
    {
        DestroyPanel();
    }

    void SetPopulationText(int population)
    {
        CulturePop.text = population < 1000 ?
            population.ToString() :
            DisplayUtils.RoundToKilo(population).ToString();
    }

    void SetNameText(string name)
    {
        CultureName.text = name;
    }

    void SetColor(Color color)
    {
        CultureColor.color = color;
    }

    public void DestroyPanel()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Culture.OnPopulationChanged -= Culture_OnPopulationChanged;
        Culture.OnNameChanged -= Culture_OnNameChanged;
        Culture.OnColorChanged -= Culture_OnColorChanged;
        Culture.OnCultureDestroyed -= Culture_OnDestroyed;
    }

}
