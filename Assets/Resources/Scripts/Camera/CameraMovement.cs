using UnityEngine;


public class CameraMovement
{

    public readonly Rect NewBounds;
    public readonly float ZoomLevel;

    public Rect NewCameraEdges { get; }
    public Camera Camera { get; }
    public Vector3 NewOrigin { get; }
    public Rect ActualPosition { get; }
    public float NewZoom { get; }

    public CameraMovement(Rect newBounds, Camera cam)
    {
        NewBounds = newBounds;
        Camera = cam;

        NewZoom = newBounds.height / 2;
        ActualPosition = new Rect(newBounds.min, new Vector2(newBounds.height * Camera.aspect, newBounds.height));
        NewOrigin = ActualPosition.center;

    }

    public void ExecuteMove()
    {
        Camera.transform.position = NewOrigin;
        Camera.orthographicSize = NewZoom;
    }
}

