using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CultureHelperMethods 
{
    public static float GetCultureDistance(Culture firstCulture, Culture secondCulture)
    {
        Color first = firstCulture.Color;
        Color second = secondCulture.Color;

        float colorDistanceSquared = GetColorDistance(first, second);
        float colorDistanceNormalized = Mathf.Lerp(0, 3, colorDistanceSquared);
        return colorDistanceNormalized;
    }

    public static float GetColorDistance(Color first, Color second)
    {
        return Mathf.Pow((first.r - second.r), 2) + Mathf.Pow((first.g - second.g), 2) + Mathf.Pow((first.b - second.b), 2);
    }
}
