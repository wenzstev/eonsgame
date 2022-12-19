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

        Rect newBounds = attemptedMove.NewCameraEdges;

        if(newBounds.x < BoardEdges.Bounds.x)
        {
            newBounds.x = BoardEdges.Bounds.x;
        }

        if(newBounds.xMax > BoardEdges.Bounds.xMax)
        {
            newBounds.x = BoardEdges.Bounds.xMax - newBounds.width;
        }

        if(newBounds.y < BoardEdges.Bounds.y)
        {
            newBounds.y = BoardEdges.Bounds.y;
        }

        if (newBounds.yMax > BoardEdges.Bounds.yMax)
        {
            newBounds.y = BoardEdges.Bounds.yMax - newBounds.height;
        }

        return new CameraMovement(newBounds, attemptedMove.Camera);

    }




}
