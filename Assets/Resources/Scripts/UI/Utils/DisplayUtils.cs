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
