using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PerlinHeightmapGenerator : HeightmapGenerator
{
    public int octaves;
    public float persistance;
    public float lacunarity;
    public float scale;
    public int seed;

    public float heightBuffer;

    public float WaterLevel = .1f;
    public float AvgAboveWaterElevation = .1f;
    public float SeaLevelRiseSteepness = 15;

    public override float LandRisePoint{ get; set; }
    public override float NormalizedSeaLevel { get { return WaterLevel; } }


    CompoundCurve heightModifier;
    public CompoundCurve HeightModifier
    {
        get
        {
            if(heightModifier == null)
            {
                SigmoidCurve firstComponent = new SigmoidCurve(AvgAboveWaterElevation + WaterLevel, LandRisePoint, -SeaLevelRiseSteepness);
                ExponentialCurve secondComponent = new ExponentialCurve(30);
                heightModifier = new CompoundCurve(new List<ICurve>() { firstComponent, secondComponent });
            }
            return heightModifier;
        }
    }



    //public AnimationCurve heightModifier;


    public override float[,] CreateHeightmap(int width, int height)
    {
        float minNoiseValue = float.MinValue;
        float maxNoiseValue = float.MaxValue;

        float[,] points = new float[width, height];

        Random.InitState(System.DateTime.Now.Millisecond); // TODO: move this to allow for user editing of seed
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
                points[x, y] = HeightModifier.GetPointOnCurve(Mathf.InverseLerp(minNoiseValue, maxNoiseValue, points[x, y]));
            }
        }

        return points;
    }
}
