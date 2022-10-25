using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedTile : SerializedGameObject
{
    public SerializedTile(GameObject tile)
    {
        serializedComponents.Add(JsonUtility.ToJson(tile.GetComponent<TileChars>()));
    }
}


[System.Serializable]
public struct SerializedTileChars
{
    public int x;
    public int y;
    public bool isCoast;
    public float absoluteHeight;
    public float humidity;
    public float temperature;

    public SerializedTileChars(TileChars tc)
    {
        x = tc.x;
        y = tc.y;
        absoluteHeight = tc.absoluteHeight;
        isCoast = tc.isCoast;
        humidity = tc.humidity;
        temperature = tc.temperature;
    }
}