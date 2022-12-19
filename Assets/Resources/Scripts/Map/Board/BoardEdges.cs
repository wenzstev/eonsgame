using System;
using System.Collections;
using UnityEngine;


public class BoardEdges
{
    public Vector2 TopLeft { get; private set; }
    public Vector2 TopRight { get; private set; }
    public Vector2 BottomRight { get; private set; }
    public Vector2 BottomLeft { get; private set; }

    public Rect Bounds { get; private set; }


    public BoardEdges(int height, int width, float tileDimension)
    {
        float boardHeight = height * tileDimension;
        float boardWidth = width * tileDimension;

        BottomLeft = new Vector2(0, 0);
        TopLeft = new Vector2(0, boardHeight);
        TopRight = new Vector2(boardWidth, boardHeight);
        BottomRight = new Vector2(boardWidth, 0);

        Bounds = new Rect(TopLeft.x, TopLeft.y, height, width);

    }

 
}
