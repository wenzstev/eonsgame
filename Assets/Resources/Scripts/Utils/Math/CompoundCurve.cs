using System.Collections.Generic;
using UnityEngine;

public class CompoundCurve : ICurve
{
    public List<ICurve> Curves;

    public CompoundCurve(List<ICurve> curves)
    {
        Curves = curves;
    }


    public float GetPointOnCurve(float x)
    {
        float val = 0;
        foreach(ICurve curve in Curves)
        {
            val += curve.GetPointOnCurve(x);

        }
        return val;
    }
}