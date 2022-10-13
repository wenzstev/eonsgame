using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedTile
{
    public int x;
    public int y;
    public int type;

    public List<SerializedCulture> cultures;

    public SerializedTile(GameObject tile, int x, int y)
    {
        this.x = x;
        this.y = y;

        cultures = new List<SerializedCulture>();

        TileInfo ti = tile.GetComponent<TileInfo>();
        type = (int)ti.tileType;

        foreach (Culture c in ti.cultures.Values)
        {
            cultures.Add(new SerializedCulture(c));
        }
    }

}