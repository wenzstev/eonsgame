using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStats : MonoBehaviour
{
    public int equator
    {
        get
        {
            return isHemisphere ? 0 : Mathf.FloorToInt(height / 2);
        }
    }
    public int maxDistFromEquator
    {
        get
        {
            return isHemisphere ? height : Mathf.FloorToInt(height / 2);
        }
    }
    public float seaLevel
    {
        get
        {
            return percentUnderWater * elevationRange;
        }
    }



    public int height;
    public int width;

    public float tempVariance = 25f;

    public float percentUnderWater;
    public float globalTemp;
    public float globalHumidity;
    public bool isHemisphere = false;
 

    public float elevationRange;


    public GameObject[] tileTypes;



}
