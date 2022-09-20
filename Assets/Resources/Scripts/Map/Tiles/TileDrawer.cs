using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDrawer : MonoBehaviour
{
    TileChars tileChars;
    SpriteRenderer sr;

    public Color tempMin;
    public Color tempMax;
    public Color humidityMin;
    public Color humidityMax;
    public Color waterColor;
    public Color frozenColor;

    public float tempMinValue;
    public float tempMaxValue;

    public float precipitationMinValue;
    public float precipitationMaxValue;

    // Start is called before the first frame update
    void Start()
    {
        tileChars = GetComponent<TileChars>();
        sr = GetComponent<SpriteRenderer>();
        SetColorBasedOnChars();
    }

    public void UpdateDraw()
    {
        SetColorBasedOnChars();
    }

    void SetColorBasedOnChars()
    {
        if (tileChars.isFrozenOver)
        {
            sr.color = frozenColor;
            return;
        }
        if (tileChars.isUnderwater)
        {
            sr.color = waterColor;
            return;
        }

        float lerpedTempValue = Mathf.InverseLerp(tempMinValue, tempMaxValue, tileChars.temperature);
        Color tempColor = Color.Lerp(tempMin, tempMax, lerpedTempValue);

        float lerpedPrecipitationValue = Mathf.InverseLerp(precipitationMinValue, precipitationMaxValue, tileChars.humidity);
        Color precipitationColor = Color.Lerp(humidityMin, humidityMax, lerpedPrecipitationValue);

        sr.color = Color.Lerp(tempColor, precipitationColor, .9f);

    }
}
