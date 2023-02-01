using System.Text.RegularExpressions;
using UnityEngine;

public class DisplayUtils
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

    /// <summary>
    /// Rounds the given value to the desired number of decimal points, eg 12.3456 becomes 12.3
    /// </summary>
    /// <param name="val"></param>
    /// <param name="numDecimals"></param>
    /// <returns></returns>
    public static float RoundToDecimal(float val, int numDecimals)
    {
        int largeVal = Mathf.RoundToInt(val * Mathf.Pow(10, numDecimals));
        return (float)largeVal / Mathf.Pow(10, numDecimals);
    }


    /// <summary>
    /// Break up a string in CamelCase, so that it has spaces (NewStringVal => New String Val)
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string SplitCamelCase(string str)
    {
        return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
    }
}
