using System.Collections;
using UnityEngine;


public abstract class HeightmapGenerator : MonoBehaviour
{
    public abstract float[,] CreateHeightmap(int width, int height);
}
