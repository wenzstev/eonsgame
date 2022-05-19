using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{

    public Vector2 startPosition;
    public float scale;




    // creates a perlin board and levels it based on the number of levels requested
    public int[,] getLevelledBoard(int numLevels, int boardWidth, int boardHeight)
    {
        float[,] perlinBoard = createPerlinBoard(startPosition, scale, boardWidth, boardHeight);

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

    float[,] createPerlinBoard(Vector2 startPosition, float scale, int sampleNumX, int sampleNumY)
    {
        float[,] points = new float[sampleNumX, sampleNumY];
        for(int i = 0; i < sampleNumY; i++)
        {
            for(int j = 0; j < sampleNumX; j++)
            {
                float currentSampleX = Mathf.Lerp(startPosition.x, startPosition.x + scale, Mathf.InverseLerp(0, sampleNumX, j));
                float currentSampleY = Mathf.Lerp(startPosition.y, startPosition.y + scale, Mathf.InverseLerp(0, sampleNumY, i));
                points[j, i] = Mathf.PerlinNoise(currentSampleX, currentSampleY);
            }
        }

        return points;
    }

}
