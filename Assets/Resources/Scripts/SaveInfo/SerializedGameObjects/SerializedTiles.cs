using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedTiles
{
   public List<SerializedTile> tiles;

    public SerializedTiles(Board b)
    {
        for (int y = 0; y < b.Height; y++)
        {
            for (int x = 0; x < b.Width; x++)
            {
                tiles.Add(new SerializedTile(b.tiles.GetTile(x, y), x, y));
            }
        }
    }
}