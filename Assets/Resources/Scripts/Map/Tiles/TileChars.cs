using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChars : MonoBehaviour
{
    public int x;
    public int y;

    public float elevation;
    float absoluteHeight;
    public float humidity;
    public float temperature;
    public bool isUnderwater;
    BoardStats boardStats;

    public TileChars(BoardStats bs, float h, int x, int y)
    {
        boardStats = bs;
        absoluteHeight = h * bs.elevationRange;
        elevation = Mathf.Max(0, absoluteHeight - bs.seaLevel);
        this.x = x;
        this.y = y;

        GenerateStats();
    }

    public void GenerateStats()
    {
        if (elevation == 0)
        {
            isUnderwater = true;
        }


        // to get temperature, function of height and proximity to equator

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
