using UnityEngine;
public class PowerCurve : ICurve
{
    public float Steepness { get; private set; }
    public float XOffset { get; private set; }
    int isNegative;

    public PowerCurve(float steepness, float xOffset)
    {
        isNegative = steepness < 0 ? -1 : 1;
        Steepness = Mathf.Abs(steepness);
        XOffset = xOffset;
    }

    public float GetPointOnCurve(float x)
    {
        //Debug.Log($"Steepness: {Steepness} XOffset: {XOffset}");
        //Debug.Log("calculating power point at " + x + ": " + Mathf.Pow(Steepness, x - XOffset));
        return Mathf.Pow(Steepness, x - XOffset) * isNegative;
    }
}