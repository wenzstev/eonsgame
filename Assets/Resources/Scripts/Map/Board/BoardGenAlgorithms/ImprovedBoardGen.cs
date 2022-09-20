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
        bs.GetComponent<Board>().tiles = perlinBoard;

        // step 2: temperature
        CalculateTemperatures(perlinBoard.tiles);

        // step 3: humidity
        CalculateHumidity(perlinBoard.tiles);

        // step 4: determine tile types (with sprites)
        //DetermineTileTypes(perlinBoard.tiles);

        return perlinBoard;
    }


    void CalculateHumidity(GameObject [,] tiles)
    {
        HashSet<GameObject> passedTiles = new HashSet<GameObject>();
        List<GameObject> nextTiles = new List<GameObject>();

        // step 1: find all coasts, give "rain" porportionate to amount of adjacent ocean tiles
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                TileChars curTileChars = tiles[x, y].GetComponent<TileChars>();
                if (curTileChars.isUnderwater)
                {
                    passedTiles.Add(curTileChars.gameObject);
                    continue;
                }

                Tile curTile = tiles[x, y].GetComponent<Tile>();
                int numAdjacentOceanTiles = 0;

                for (int i = 0; i < 7; i++)
                {
                    if(curTile.GetNeighbor((Direction)i) && curTile.GetNeighbor((Direction)i).GetComponent<TileChars>().isUnderwater) // if there is a neighbor in that direction, and that neighbor is underwater
                    {
                        numAdjacentOceanTiles++;
                    }
                    curTileChars.humidity = numAdjacentOceanTiles * boardObj.GetComponent<BoardStats>().globalHumidity;
                }

                passedTiles.Add(curTileChars.gameObject);     
            }
        }

        // step 2: iterate through adjacent tiles in breadth-first algorithm
    }


    float CalculateHumidity(TileChars tileChars)
    {


        return 0;
    }

    void CalculateTemperatures(GameObject[,] tiles)
    {
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                tiles[x,y].GetComponent<TileChars>().temperature = CalculateTileTemperature(tiles[x, y].GetComponent<TileChars>());
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


}
