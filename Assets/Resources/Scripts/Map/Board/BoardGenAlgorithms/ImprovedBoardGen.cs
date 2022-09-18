using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImprovedBoardGen : BoardGenAlgorithm
{
    const float SCALE = 5f;

    GameObject boardObj;


    public override BoardTileRelationship CreateBoard(BoardStats bs)
    {
        boardObj = bs.gameObject;
        // step 1: heightmap
        BoardTileRelationship perlinBoard = CreateRawPerlinBoard(boardObj, SCALE, bs.width, bs.height);

        // step 2: temperature
        CalculateTemperatures(perlinBoard.tiles);

        // step 3: humidity

        // step 4: determine tile types (with sprites)
        DetermineTileTypes(perlinBoard.tiles);

        return perlinBoard;
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

    float calculateHumidity(GameObject [,] tiles)
    {

        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
            }
        }

        return 0;
    }

    void CalculateTemperatures(GameObject[,] tiles)
    {
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                CalculateTileTemperature(tiles[x, y].GetComponent<TileChars>());
            }
        }
    }

    float CalculateTileTemperature(TileChars tileChars)
    {
        BoardStats boardStats = boardObj.GetComponent<BoardStats>();
        // average temperature is at middle latitudes
        float distanceFromEquator = Mathf.Abs(boardStats.equator - tileChars.y) / (float)boardStats.maxDistFromEquator;
        float tempWithoutElevation = boardStats.globalTemp - (distanceFromEquator * boardStats.tempVariance) + boardStats.tempVariance / 2;

        // TODO: mediating effects of water?

        return tempWithoutElevation - tileChars.elevation / 100; // elevation is in meters, lose 1 degree Celcius per 100 meters in height change
    }

    void DetermineTileTypes(GameObject[,] tiles)
    {
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                // TODO determine the right type of tile to exist here, maybe from scriptableobject?
            }
        }
    }
}
