using UnityEngine;
public class PowerCurve : ICurve
{
    public float Steepness { get; private set; }
    public float XOffset { get; private set; }

    public PowerCurve(float steepness, float xOffset)
    {
        Steepness = steepness;
        XOffset = xOffset;
    }

    public float GetPointOnCurve(float x)
    {
        return Mathf.Pow(Steepness, x - XOffset);
    }
}