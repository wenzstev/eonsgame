using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileCharsPanelController : MonoBehaviour
{
    public TextMeshProUGUI Precipitation;
    public TextMeshProUGUI Temperature;
    public TextMeshProUGUI Elevation;

    TileChars tileChars;

    public void SetValues(TileChars tc)
    {
        tileChars = tc;

        Precipitation.text = DisplayUtils.RoundToDecimal(tc.precipitation, 1).ToString();
        Temperature.text = DisplayUtils.RoundToDecimal(tc.temperature, 1).ToString();
        Elevation.text = DisplayUtils.RoundToDecimal(tc.elevation, 1).ToString();
    }
}
