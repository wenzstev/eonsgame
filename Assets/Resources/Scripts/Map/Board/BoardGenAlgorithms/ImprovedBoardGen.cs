using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImprovedBoardGen : BoardGenAlgorithm
{
    const float SCALE = 2f;


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
                tiles[x, y] = new TileChars(bs, heightMap[x,y]);
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
        BoardStats boardStats;

        public TileChars(BoardStats bs, float h)
        {
            boardStats = bs;
            height = h;

            GenerateStats();
        }

        public void GenerateStats()
        {
            if(height < boardStats.waterLevel)
            {
                isUnderwater = true;
                return;
            }


            // to get temperature, function of height and proximity to equator


        }

        public int GetTileType()
        {
            return isUnderwater ? 0 : 1;
        }
    }
}
