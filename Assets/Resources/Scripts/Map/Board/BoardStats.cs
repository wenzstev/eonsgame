using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStats : MonoBehaviour
{

    public int height;
    public int width;

    public int numLevels {
        get
        {
            return tileTypes.Length;
        }
    }

    public float waterLevel;
    public float globalTemp;
    public float globalHumidity;
    public bool isHemisphere = false;
    public int equator;

    public GameObject[] tileTypes;


}
