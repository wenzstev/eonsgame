using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrigUtils 
{
    /// <summary>
    /// Get the location on the edge of a circle at angle theta
    /// </summary>
    /// <param name="radius">Radius of the circle.</param>
    /// <param name="theta">Angle, in Radians</param>
    /// <returns></returns>
    public static Vector2 GetLocationOnCircleRadians(float radius, float theta)
    {
        return new Vector2(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta));
    }

    /// <summary>
    /// Get the location on the edge of a circle at angle theta, in degrees
    /// </summary>
    /// <param name="radius">Radius of the circle</param>
    /// <param name="theta">Angle, in degrees</param>
    /// <returns></returns>
    public static Vector3 GetLocationOnCircleDegrees(float radius, float theta)
    {
        float radTheta = Mathf.Deg2Rad * theta;
        return GetLocationOnCircleRadians(radius, radTheta);
    }
}
