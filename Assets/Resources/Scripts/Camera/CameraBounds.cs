using System;
using System.Collections.Generic;
using UnityEngine;


public class CameraBounds : MonoBehaviour, ICameraRestriction
{
    public BoardEdges BoardEdges { get { return boardStats.BoardEdges; } }
    public BoardLoader BoardLoader;
    BoardStats boardStats;
    public CameraMoveController CameraMoveController;
    public float BufferZone = 3f;

    Rect BufferEdges;



    private void Start() {
        if(BoardLoader) BoardLoader.OnBoardCreated += CameraBounds_OnBoardCreated;
    }

    public void CameraBounds_OnBoardCreated(object sender, BoardLoader.OnBoardCreatedEventArgs e) {
        SetBoard(e.BoardStats);
        CameraMoveController.AttemptMove(new CameraMovement(BoardEdges.Bounds.center, CameraMoveController.Camera));
    }

    public void SetBoard(BoardStats bs)
    {
        boardStats = bs;
        CameraMoveController.AddRestriction(this);
        Vector2 BufferZoneVector = new Vector2(BufferZone, BufferZone);
        BufferEdges = new Rect(BoardEdges.BottomLeft - BufferZoneVector, BoardEdges.TopRight +  2 * BufferZoneVector);
    }


    public CameraMovement ProvideModifiedMove(CameraMovement attemptedMove)
    {
        if (boardStats == null) Debug.LogError("You need to initialize the CameraBounds with a board!");

        /*
         * Check if new camera bounds are outside of the board
         * if so
         *      set edges of camera bounds to edge of the board
         *      return new movement with these bounds
         * else 
         *      return original, unchanged movement
         * 
         * 
         * if topleft is out of bounds
         *      recreate where topleft is in bounds
         * if bottomRight is out of bounds
         *      recreate where bottomright is in bounds
         * 
         */

        Rect newBounds = attemptedMove.ActualPosition;

        // check bottom, right, top, left in this order to ensure that top left corner trumps the rest

        if (newBounds.y < BufferEdges.y)
        {
            newBounds.y = BufferEdges.y;
        }

        // should camera bounds worry about the righthand side of the rectangle? 
        // yes, but should be set first, so that it can be overridden by the righthand side if the whole bounds is too small
        if (newBounds.xMax > BufferEdges.xMax)
        {
            newBounds.x = BufferEdges.xMax - newBounds.width;
        }


        if (newBounds.yMax > BufferEdges.yMax)
        {
            newBounds.y = BufferEdges.yMax - newBounds.height;
        }


        if (newBounds.x < BufferEdges.x)
        {
            newBounds.x = BufferEdges.x;
        }

        return new CameraMovement(newBounds, attemptedMove.Camera);
    }




}
