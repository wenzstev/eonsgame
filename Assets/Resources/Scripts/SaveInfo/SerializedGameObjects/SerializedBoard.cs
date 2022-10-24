using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SerializedBoard 
{
    public int width;
    public int height;

    


    public SerializedBoard(Board b)
    {
        width = b.Width;
        height = b.Height;
    }
}