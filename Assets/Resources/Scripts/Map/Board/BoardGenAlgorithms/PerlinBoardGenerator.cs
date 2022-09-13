using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinBoardGenerator : BoardGenAlgorithm
{

    public float scale;

    public bool randomCoords;


    // creates a perlin board and levels it based on the number of levels requested
    public override int[,] getLevelledBoard(int numLevels, int boardWidth, int boardHeight)
    {
        float[,] perlinBoard = createPerlinBoard(scale, boardWidth, boardHeight);

        int[,] levelledBoard = new int[perlinBoard.GetLength(0), perlinBoard.GetLength(1)];

        for (int j = 0; j < levelledBoard.GetLength(1); j++)
        {
            for (int i = 0; i < levelledBoard.GetLength(0); i++)
            {
                levelledBoard[i, j] = Mathf.FloorToInt(perlinBoard[i, j] * numLevels);
            }
        }
        return levelledBoard;
    }



}
