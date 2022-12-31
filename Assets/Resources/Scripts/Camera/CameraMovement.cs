using UnityEngine;


public class CameraMovement
{

    public readonly Rect NewBounds;
    public readonly float ZoomLevel;

    public Camera Camera { get; }
    public Vector3 NewOrigin { get { return new Vector3(ActualPosition.center.x, ActualPosition.center.y, Camera.transform.position.z); } }
    public Rect ActualPosition { get { return new Rect(NewBounds.min, new Vector2(NewBounds.height * Camera.aspect, NewBounds.height)); } } // preserve the height, but not the width 
    public float NewZoom { get { return NewBounds.height / 2; } }

    public CameraMovement(Rect newBounds, Camera cam)
    {
        if(!cam.orthographic)
        {
            Debug.LogError("Camera must be orthographic!");
            return;
        }

        NewBounds = newBounds;
        Camera = cam;
    }

    public CameraMovement(Vector3 newOrigin, Camera cam)
    {
        if (!cam.orthographic)
        {
            Debug.LogError("Camera must be orthographic!");
            return;
        }
        Camera = cam;
        float height = Camera.orthographicSize * 2;
        float width = height * Camera.aspect;

        NewBounds = new Rect(new Vector2(newOrigin.x - width / 2, newOrigin.y - height / 2), new Vector2(width, height));        
    }

    public CameraMovement(float newZoom, Camera cam)
    {
        if (!cam.orthographic)
        {
            Debug.LogError("Camera must be orthographic!");
            return;
        }
        Camera = cam;

        Vector3 newOrigin = Camera.transform.position;
        float height = newZoom * 2;
        float width = height * Camera.aspect;

        NewBounds = new Rect(new Vector2(newOrigin.x - width / 2, newOrigin.y - height / 2), new Vector2(width, height));

    }

    public void ExecuteMove()
    {
        Camera.transform.position = NewOrigin;
        Camera.orthographicSize = NewZoom;
    }
}

