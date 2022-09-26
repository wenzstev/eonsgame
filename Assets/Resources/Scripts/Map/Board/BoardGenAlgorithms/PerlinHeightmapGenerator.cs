using System.Collections;
using UnityEngine;
using System.Linq;

public class PerlinHeightmapGenerator : HeightmapGenerator
{
    public int octaves;
    public float persistance;
    public float lacunarity;
    public float scale;
    public int seed;
    public AnimationCurve heightModifier;


    public override float[,] CreateHeightmap(int width, int height)
    {
        float minNoiseValue = float.MinValue;
        float maxNoiseValue = float.MaxValue;

        float[,] points = new float[width, height];

        Random.InitState(seed);
        Vector2 startPoint = new Vector2(Random.Range(0, 10000), Random.Range(0, 10000));
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                float value = 0;
                float curAmplitude = 1;
                float curFrequency = 1;
                for(int i = 0; i < octaves; i++)
                {
                    float sampleX = (startPoint.x + x) / scale * curFrequency;
                    float sampleY = (startPoint.y + y) / scale * curFrequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    value += perlinValue * curAmplitude;
                    curAmplitude *= persistance;
                    curFrequency *= lacunarity;
                }

                points[x, y] = value;

                minNoiseValue = Mathf.Max(value, minNoiseValue);
                maxNoiseValue = Mathf.Min(value, maxNoiseValue);
                //float[] val = samplePoints.Select((p, i) => Mathf.PerlinNoise(p.x, p.y) * (1 / Mathf.Pow(i+1, 2))).ToArray();
                //points[x, y] = samplePoints.Select((p, i) => Mathf.PerlinNoise(p.x, p.y) * (1f/(i+1f))).Sum();
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                points[x, y] = heightModifier.Evaluate(Mathf.InverseLerp(minNoiseValue, maxNoiseValue, points[x, y]));
            }
        }

        return points;
    }
}
