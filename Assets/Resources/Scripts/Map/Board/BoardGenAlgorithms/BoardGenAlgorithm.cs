using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardGenAlgorithm : MonoBehaviour
{
    public abstract int[,] getLevelledBoard(BoardStats bs);

    protected float[,] createPerlinBoard(float scale, int sampleNumX, int sampleNumY)
    {

        Vector2 startPosition = new Vector2(Random.value * 10, Random.value * 10);
        float[,] points = new float[sampleNumX, sampleNumY];
        for (int i = 0; i < sampleNumY; i++)
        {
            for (int j = 0; j < sampleNumX; j++)
            {
                float currentSampleX = Mathf.Lerp(startPosition.x, startPosition.x + scale, Mathf.InverseLerp(0, sampleNumX, j));
                float currentSampleY = Mathf.Lerp(startPosition.y, startPosition.y + scale, Mathf.InverseLerp(0, sampleNumY, i));
                points[j, i] = Mathf.PerlinNoise(currentSampleX, currentSampleY);
            }
        }

        return points;
    }

}
