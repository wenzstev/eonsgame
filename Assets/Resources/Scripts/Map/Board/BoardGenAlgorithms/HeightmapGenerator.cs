using System.Collections;
using UnityEngine;


public abstract class HeightmapGenerator : MonoBehaviour
{
    public abstract float LandRisePoint { get; set; }
    public abstract float NormalizedSeaLevel { get; }
    public abstract float[,] CreateHeightmap(int width, int height);
}
