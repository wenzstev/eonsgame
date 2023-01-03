using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SerializedCultures
{

    public List<SerializedCulture> StagedCultures;
    public List<SerializedCulture> SettledCultures;

    public SerializedCultures(Board b)
    {
        StagedCultures = new List<SerializedCulture>();
        SettledCultures = new List<SerializedCulture>();

        for(int j = 0; j < b.Height; j++)
        {
            for(int i = 0; i < b.Width; i++)
            {
                foreach(Culture c in b.Tiles[i, j].GetComponentInChildren<CultureHandler>().GetAllSettledCultures())
                {
                    SettledCultures.Add(new SerializedCulture(c, i, j));
                }
                foreach(Culture c in b.Tiles[i, j].GetComponentInChildren<CultureHandler>().GetAllStagedCultures())
                {
                    StagedCultures.Add(new SerializedCulture(c, i, j));
                }
            }
        }
    }
}