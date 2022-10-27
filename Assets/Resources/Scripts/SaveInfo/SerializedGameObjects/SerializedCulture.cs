using System.Collections;
using UnityEngine;

[System.Serializable]
public class SerializedCulture : SerializedGameObject
{
    public int x;
    public int y;


    public SerializedCulture(Culture culture, int x, int y)
    {
        this.x = x;
        this.y = y;
        serializedComponents.Add(JsonUtility.ToJson(culture));
        serializedComponents.Add(JsonUtility.ToJson(culture.GetComponent<CultureMemory>()));
    }

}