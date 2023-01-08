using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CultureListButton : MonoBehaviour
{
    Culture Culture;

    public TextMeshProUGUI CultureNameText;
    public TextMeshProUGUI CulturePopulationText;
    public Image CultureColor;

    public event EventHandler<OnCultureButtonClickedEventArgs> OnCultureButtonClicked;

    public void Initialize(Culture c)
    {
        Culture = c;
        CultureNameText.text = c.Name;
        CulturePopulationText.text = c.Population.ToString();
        CultureColor.color = c.Color;
    }

    public void CultureButtonClicked()
    {
        OnCultureButtonClicked?.Invoke(this, new OnCultureButtonClickedEventArgs() { Culture = Culture });
    }


    public class OnCultureButtonClickedEventArgs : EventArgs
    {
        public Culture Culture;
    }
}
