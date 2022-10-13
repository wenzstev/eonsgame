using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileChars : MonoBehaviour
{
    public int x;
    public int y;

    public float elevation
    {
        get
        {
            return Mathf.Max(0, absoluteHeight - boardStats.seaLevel);
        }
    }

    public bool isUnderwater
    {
        get
        {
            return elevation == 0;
        }
    }

    public bool isFrozenOver
    {
        get
        {
            float threshold = isUnderwater ? -5 : 0;
            return temperature < threshold;
        }
    }


    public bool isCoast;
    public float absoluteHeight;
    public float humidity;
    public float temperature;

    [System.NonSerialized]
    public BoardStats boardStats;
}