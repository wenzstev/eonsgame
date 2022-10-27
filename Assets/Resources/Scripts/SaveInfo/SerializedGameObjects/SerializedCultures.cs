using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SerializedCultures
{
    public List<SerializedCulture> cultures;

    public SerializedCultures(Board b)
    {
        cultures = new List<SerializedCulture>();
        for(int j = 0; j < b.Height; j++)
        {
            for(int i = 0; i < b.Width; i++)
            {
                foreach(KeyValuePair<string, Culture> kvp in b.Tiles[i,j].GetComponent<TileInfo>().cultures)
                {
                    cultures.Add(new SerializedCulture(kvp.Value, i, j));
                }
            }
        }
    }
}