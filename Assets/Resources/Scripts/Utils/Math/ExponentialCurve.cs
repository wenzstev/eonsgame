using UnityEngine;

public class ExponentialCurve : ICurve
{
    public float Power;

    public ExponentialCurve(float p)
    {
        Power = p;
    }


    public float GetPointOnCurve(float x)
    {
        return Mathf.Pow(x, Power);
    }
}