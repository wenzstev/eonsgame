using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CultureListButton : MonoBehaviour
{
    public TextMeshProUGUI CultureNameText;
    public TextMeshProUGUI CulturePopulationText;
    public Image CultureColor;

    public void Initialize(Culture c)
    {
        CultureNameText.text = c.Name;
        CulturePopulationText.text = c.Population.ToString();
        CultureColor.color = c.Color;
    }


}
