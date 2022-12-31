using System;
using System.Collections;
using UnityEngine;


public class CameraBounds : MonoBehaviour, ICameraRestriction
{
    public BoardEdges BoardEdges { get { return boardStats.BoardEdges; } }
    BoardStats boardStats;
    public CameraMoveController CameraMoveController;
    public float BufferZone = 3f;

   
    public void SetBoard(BoardStats bs)
    {
        boardStats = bs;
        CameraMoveController.AddRestriction(this); 
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

        if (newBounds.y < BoardEdges.Bounds.y)
        {
            newBounds.y = BoardEdges.Bounds.y;
        }

        // should camera bounds worry about the righthand side of the rectangle? 
        // yes, but should be set first, so that it can be overridden by the righthand side if the whole bounds is too small
        if (newBounds.xMax > BoardEdges.Bounds.xMax)
        {
            Debug.Log("newbounds is too big");
            Debug.Log($"{BoardEdges.Bounds.xMax} - {newBounds.width} = {BoardEdges.Bounds.xMax - newBounds.width}");
            newBounds.x = BoardEdges.Bounds.xMax - newBounds.width;
        }


        if (newBounds.yMax > BoardEdges.Bounds.yMax)
        {
            newBounds.y = BoardEdges.Bounds.yMax - newBounds.height;
        }


        if (newBounds.x < BoardEdges.Bounds.x)
        {
            newBounds.x = BoardEdges.Bounds.x;
        }

        return new CameraMovement(newBounds, attemptedMove.Camera);
    }




}
