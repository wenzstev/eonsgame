using System.Collections;
using UnityEngine;


[System.Serializable]
public struct GompertzCurve : ICurve
{
    public float Asymptote;
    public float XDisplacement;
    public float GrowthRate;
    public float YDisplacement;


    public GompertzCurve(float asy, float xdis, float growth, float ydis)
    {
        Asymptote = asy;
        XDisplacement = xdis;
        GrowthRate = growth;
        YDisplacement = ydis;
    }


    public float GetPointOnCurve(float x)
    {
        float cValue = -GrowthRate * x;
        float bValue = -XDisplacement * Mathf.Exp(cValue);
        float aValue = Asymptote * Mathf.Exp(bValue);
        float value = YDisplacement + aValue;
        return value;
    }



}
