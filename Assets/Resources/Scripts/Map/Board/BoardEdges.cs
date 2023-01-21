using System;
using System.Collections;
using UnityEngine;


public class BoardEdges
{
    public Vector2 TopLeft
    {
        get { return new Vector2(Bounds.xMin, Bounds.yMax); }
    }
    public Vector2 TopRight
    { 
        get { return new Vector2(Bounds.xMax, Bounds.yMax); }
    }
    public Vector2 BottomRight
    {
        get { return new Vector2(Bounds.xMax, Bounds.yMin); }
    }
    public Vector2 BottomLeft
    {
        get { return new Vector2(Bounds.xMin, Bounds.yMin); }
    }

    public Rect Bounds { get; private set; }


    public BoardEdges(int height, int width, float tileDimension)
    {
        float halfTile = tileDimension / 2;
<<<<<<< HEAD
        Bounds = new Rect(0-halfTile, 0-halfTile, width, height);
=======
        Bounds = new Rect(0-halfTile, 0-halfTile, width+halfTile, height+halfTile);
>>>>>>> 9110bf8fe4618a00a695e102b0305ad6ac2df074
    }

 
}
