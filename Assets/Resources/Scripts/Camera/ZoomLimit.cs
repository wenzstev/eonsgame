using System.Collections;
using UnityEngine;

public class ZoomLimit : MonoBehaviour, ICameraRestriction
{
    public float BufferZone;

    [SerializeField]
    BoardLoader boardLoader;

    [SerializeField]
    CameraMoveController CameraMoveController;

    BoardEdgesGetter boardEdgesGetter;
    BoardEdges BoardEdges { get { return boardEdgesGetter.BoardEdges; } }

    Rect BufferEdges;

    private void Start()
    {
        if (boardLoader) boardEdgesGetter = new BoardEdgesGetter(boardLoader);
        CameraMoveController.AddRestriction(this);

    }
    void SetBufferEdges()
    {
        Vector2 BufferZoneVector = new Vector2(BufferZone, BufferZone);
        BufferEdges = new Rect(BoardEdges.BottomLeft - BufferZoneVector, BoardEdges.TopRight + 2 * BufferZoneVector);
    }

    public CameraMovement ProvideModifiedMove(CameraMovement attemptedMove)
    {
        if (BufferEdges.height == 0) SetBufferEdges();

        Rect newBounds = attemptedMove.ActualPosition;

        if(newBounds.height > BufferEdges.height)
        {
            return new CameraMovement(attemptedMove.Camera.orthographicSize, BufferEdges.center, attemptedMove.Camera);
        }
        return attemptedMove;
    }


}