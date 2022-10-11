using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SerializedBoard 
{
    public List<SerializedTile> tiles;
    public int width;
    public int height;


    public SerializedBoard(Board b)
    {
        width = b.Width;
        height = b.Height;

        tiles = new List<SerializedTile>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tiles.Add(new SerializedTile(b.tiles.GetTile(x, y), x, y));
            }
        }
    }
}