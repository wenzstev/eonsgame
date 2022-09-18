using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImprovedBoardGen : BoardGenAlgorithm
{
    const float SCALE = 5f;


    public override int[,] getLevelledBoard(BoardStats bs)
    {
        float[,] perlinBoard = createPerlinBoard(SCALE, bs.width, bs.height);
        TileChars[,] tiles = InitializeTileChars(perlinBoard, bs);
        return convertTileTypesToLevelledBoard(tiles);
    }


    TileChars[,] InitializeTileChars(float[,] heightMap, BoardStats bs)
    {
        TileChars[,] tiles = new TileChars[heightMap.GetLength(0), heightMap.GetLength(1)];

        for(int y = 0; y < tiles.GetLength(1); y++)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                tiles[x, y] = new TileChars(bs, heightMap[x,y], x, y);
            }
        }
        return tiles;
    }

    int[,] convertTileTypesToLevelledBoard(TileChars[,] tiles)
    {
        int[,] levelledBoard = new int[tiles.GetLength(0), tiles.GetLength(1)];

        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                levelledBoard[x, y] = tiles[x, y].GetTileType();
            }
        }

        return levelledBoard;
    }



    class TileChars
    {
        int x;
        int y;

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
            if(elevation == 0)
            {
                isUnderwater = true;
            }


            // to get temperature, function of height and proximity to equator
            temperature = calculateTemperature();

        }

        float calculateTemperature()
        {
            // average temperature is at middle latitudes
            float distanceFromEquator = Mathf.Abs(boardStats.equator - y) / (float) boardStats.maxDistFromEquator;
            float tempWithoutElevation = boardStats.globalTemp - (distanceFromEquator * boardStats.tempVariance) + boardStats.tempVariance/2;

            // TODO: mediating effects of water?

            return tempWithoutElevation - elevation / 100; // elevation is in meters, lose 1 degree Celcius per 100 meters in height change
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



            if(elevation > 6000f)
            {
                return 5; // 5 is mountain peak
            }

            return 1; // 1 is grassland, "default" right now
        }
    }
}
