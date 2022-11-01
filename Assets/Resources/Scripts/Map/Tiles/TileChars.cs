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

    BoardStats _boardStats;
    public BoardStats boardStats
    {
        get
        {
            if(_boardStats == null)
            {
                Debug.Log(name);
                Debug.Log(GetComponent<Tile>().board);
                _boardStats = GetComponent<Tile>().board.GetComponent<BoardStats>();
            }
            return _boardStats;
        }
    }


    public int GetTileType()
    {
        if (temperature < 0f)
        {
            return 4; // 4 is tundra
        }

        if (isUnderwater)
        {
            return 0; // 0 is ocean
        }



        if (temperature > 15f)
        {
            return 3; // 3 is desert
        }



        if (elevation > 6000f)
        {
            return 5; // 5 is mountain peak
        }

        return 1; // 1 is grassland, "default" right now
    }
}
