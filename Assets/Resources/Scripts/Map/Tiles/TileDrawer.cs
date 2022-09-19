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

    public float tempMinValue;
    public float tempMaxValue;

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
        float lerpedTempValue = Mathf.InverseLerp(tempMinValue, tempMaxValue, tileChars.temperature);
        sr.color = Color.Lerp(tempMin, tempMax, lerpedTempValue);
    }
}
