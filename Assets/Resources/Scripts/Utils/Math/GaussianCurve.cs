using UnityEngine;

public class GaussianCurve : ICurve
{
    public float MaxHeight { get; private set; }
    public float Center { get; private set; }
    public float StandardDeviation { get; private set; }

    public GaussianCurve(float maxHeight, float center, float standardDeviation)
    {
        MaxHeight = maxHeight;
        Center = center;
        StandardDeviation = standardDeviation;
    }

    public float GetPointOnCurve(float x)
    {
        //Debug.Log($"MaxHeight: {MaxHeight} Center: {Center} StandardDeviation: {StandardDeviation}");
        //Debug.Log($"value at {x}: {MaxHeight * Mathf.Exp(-Mathf.Pow(x - Center, 2) / (2 * Mathf.Pow(StandardDeviation, 2)))}");
        return MaxHeight * Mathf.Exp(-Mathf.Pow(x - Center, 2) / (2 * Mathf.Pow(StandardDeviation, 2)));
    }
}