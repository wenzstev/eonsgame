using System.Collections;
using UnityEngine;

public class TestUtils
{
    public static void SetupBasicMap()
    {
        

    }

    public static void TearDownTest()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(o);
        }
    }

}