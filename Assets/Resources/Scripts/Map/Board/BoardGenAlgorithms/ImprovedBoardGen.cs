using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImprovedBoardGen : BoardGenAlgorithm
{
    const float SCALE = 2f;

    public float waterLevel;
    public float globalTemp;
    public float globalHumidity;


    public override int[,] getLevelledBoard(int numLevels, int boardWidth, int boardHeight)
    {
        float[,] perlinBoard = createPerlinBoard(SCALE, boardWidth, boardHeight);
        TileChars[,] tiles = InitializeTileChars(perlinBoard);
        return convertTileTypesToLevelledBoard(tiles);
    }


    TileChars[,] InitializeTileChars(float[,] heightMap)
    {
        TileChars[,] tiles = new TileChars[heightMap.GetLength(0), heightMap.GetLength(1)];

        for(int y = 0; y < tiles.GetLength(1); y++)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                tiles[x, y] = new TileChars(this, heightMap[x,y]);
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
        public float height;
        public float humidity;
        public float temperature;
        public bool isUnderwater;
        ImprovedBoardGen boardInfo;

        public TileChars(ImprovedBoardGen b, float h)
        {
            boardInfo = b;
            height = h;

            GenerateStats();
        }

        public void GenerateStats()
        {
            if(height < boardInfo.waterLevel)
            {
                isUnderwater = true;
            }
        }

        public int GetTileType()
        {
            return isUnderwater ? 0 : 1;
        }
        

    }
}
