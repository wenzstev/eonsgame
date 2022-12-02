using UnityEngine;

// currently unused in favor of GaussianCurve. Gets normalized distribution. TODO: rewrite for random numbers?
public class NormalCurve : ICurve
{
    public float Mean { get; private set; }
    public float StandardDeviation { get; private set; }

    public NormalCurve(float m, float s)
    {
        Mean = m;
        StandardDeviation = s;
    }

    public float GetPointOnCurve(float x)
    {
        return (1 / (StandardDeviation * Mathf.Sqrt(2 * Mathf.PI))) * Mathf.Exp(-1 * Mathf.Pow(x - Mean, 2) / 2 * Mathf.Pow(StandardDeviation, 2));

    }
}