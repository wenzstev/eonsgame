using System.Collections;
using UnityEngine;


public class LogarithmicCurve : ICurve
{
    public float Sharpness;

    public LogarithmicCurve(float sharpness)
    {
        Sharpness = sharpness;
    }

    public float GetPointOnCurve(float x)
    {
        return Sharpness * Mathf.Log(x);
    }

}
