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

}