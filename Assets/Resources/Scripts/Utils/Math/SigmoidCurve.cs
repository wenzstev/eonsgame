using UnityEngine;

public class SigmoidCurve : ICurve
{
    public float MaxVal{ get; private set; }
    public float MidPoint { get; private set; }
    public float Steepness { get; private set; }

    public SigmoidCurve(float maxVal, float midPoint, float steepness)
    {
        MaxVal = maxVal;
        MidPoint = midPoint;
        Steepness = steepness;
    }

    public float GetPointOnCurve(float x)
    {
        return MaxVal / (1 + Mathf.Exp(Steepness * (x - MidPoint)));
    }

}