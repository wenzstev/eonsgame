using UnityEngine;
public class PowerCurve : ICurve
{
    public float Steepness { get; private set; }
    public float XOffset { get; private set; }

    public float InitialValue = 1;
    public float PowerMultiplier = 1;

    public PowerCurve(float steepness, float xOffset, float initialValue, float powerMultiplier)
    {
        if(steepness < 0) Debug.LogError("Cannot create curve of negative exponent raised to a power! Consider making the InitialValue negative instead.");
        Steepness = steepness;
        XOffset = xOffset;
        InitialValue = initialValue;
        PowerMultiplier = powerMultiplier;
    }


    public float GetPointOnCurve(float x)
    {
        //Debug.Log($"Steepness: {Steepness} XOffset: {XOffset}");
        return InitialValue * Mathf.Pow(Steepness, (x * PowerMultiplier) - XOffset);
    }
}