using System;
using System.Collections.Generic;
using UnityEngine;


public class CameraBounds : MonoBehaviour, ICameraRestriction
{
    public BoardEdges BoardEdges { get { return boardEdgesGetter.BoardEdges; } }
    public CameraMoveController CameraMoveController;
    public BoardLoader BoardLoader;
    public float BufferZone = 3f;
    public IBoardEdgesGetter boardEdgesGetter;

    Rect BufferEdges;


    private void Start() {
        if (BoardLoader) boardEdgesGetter = new BoardEdgesGetter(BoardLoader);

        boardEdgesGetter.OnBoardEdgesSet += BoardEdgesGetter_OnBoardEdgesSet;
        CameraMoveController.AddRestriction(this);
    }
    
    void SetBufferEdges()
    {
        Vector2 BufferZoneVector = new Vector2(BufferZone, BufferZone);
        BufferEdges = new Rect(BoardEdges.BottomLeft - BufferZoneVector, BoardEdges.TopRight + 2 * BufferZoneVector);
    }

    public void SetBoardEdgesGetter(IBoardEdgesGetter newGetter)
    {
        boardEdgesGetter = newGetter;
    }

    void BoardEdgesGetter_OnBoardEdgesSet(object sender, EventArgs e)
    {
        SetBufferEdges();
        CameraMoveController.AttemptMove(new CameraMovement(BufferEdges.center, Camera.main));
    }

    public CameraMovement ProvideModifiedMove(CameraMovement attemptedMove)
    {
        if (BufferEdges.width == 0) SetBufferEdges(); // initialize the rect for the first time

        if (BoardLoader == null) Debug.LogError("You need to initialize the CameraBounds with a board!");

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

        if(newBounds.width > BufferEdges.width)
        {
            newBounds.center = new Vector2(BufferEdges.center.x, newBounds.center.y);
        }

        if (newBounds.height > BufferEdges.height)
        {
            newBounds.center = new Vector2(newBounds.center.x, BufferEdges.center.y);
        }

        return new CameraMovement(newBounds, attemptedMove.Camera);
    }
}

public class BoardEdgesGetter : IBoardEdgesGetter
{
    BoardStats boardStats;
    


    public BoardEdges BoardEdges { get { return boardStats.BoardEdges; }}
    public EventHandler<EventArgs> OnBoardEdgesSet { get; set; }

    public BoardEdgesGetter(BoardLoader bl)
    {
        bl.OnBoardCreated += BoardEdgesGetter_OnBoardCreated;
    }

    void BoardEdgesGetter_OnBoardCreated(object sender, BoardLoader.OnBoardCreatedEventArgs e)
    {
        boardStats = e.BoardStats;
        OnBoardEdgesSet?.Invoke(this, EventArgs.Empty);

    }
}

public interface IBoardEdgesGetter
{
    public BoardEdges BoardEdges { get; }
    public EventHandler<EventArgs> OnBoardEdgesSet { get; set; }

}