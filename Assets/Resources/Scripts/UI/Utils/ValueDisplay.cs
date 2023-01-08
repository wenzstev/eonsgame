using UnityEngine;

public class ValueDisplay
{
    /// <summary>
    /// Rounds the give value to a power of 1000, with one decimal,
    /// e.g., 1234 becomes 1.2
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static float RoundToKilo(float val)
    {
        return Mathf.RoundToInt(val / 100) / 10f;
    }
}
