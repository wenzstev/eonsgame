using System.Collections;
using UnityEngine;

public class TestUtils
{
    public static void SetupBasicMap()
    {
        

    }

    public static void TearDownTest()
    {
        Debug.Log("clearing");
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }

    public static float ThreeDecimals(float val)
    {
        return Mathf.Round(val * 1000f) / 1000f;
    }

}