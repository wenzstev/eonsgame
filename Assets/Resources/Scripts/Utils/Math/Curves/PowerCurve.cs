using UnityEngine;
using System;

[Serializable]
public class PowerCurve : ICurve
{
    [SerializeField]
    float _steepness;

    [SerializeField]
    float _xOffset;
    public float Steepness { get { return _steepness; } }
    public float XOffset { get { return _xOffset; } }

    public float InitialValue = 1;
    public float PowerMultiplier = 1;

    public PowerCurve(float steepness, float xOffset, float initialValue, float powerMultiplier)
    {
        if(steepness < 0) Debug.LogError("Cannot create curve of negative exponent raised to a power! Consider making the InitialValue negative instead.");
        _steepness = steepness;
        _xOffset = xOffset;
        InitialValue = initialValue;
        PowerMultiplier = powerMultiplier;
    }


    public float GetPointOnCurve(float x)
    {
        //Debug.Log($"Steepness: {Steepness} XOffset: {XOffset}");
        return InitialValue * Mathf.Pow(Steepness, (x * PowerMultiplier) - XOffset);
    }
}