using System.Collections;
using UnityEngine;


[System.Serializable]
public class SerializedColor
{
    public float r;
    public float g;
    public float b;

    public SerializedColor(Color c)
    {
        this.r = c.r;
        this.g = c.g;
        this.b = c.b;
    }

    public Color UnserializeColor()
    {
        return new Color(r, g, b);
    }
}
