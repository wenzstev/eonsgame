using UnityEngine;


public class CameraZoomBounds : MonoBehaviour, ICameraRestriction
{
    public float ZoomMin = 1f;
    public float ZoomMax = 20f;

    public CameraMoveController CameraMoveController;

    private void Start() {
        CameraMoveController.AddRestriction(this);
    }

    public CameraMovement ProvideModifiedMove(CameraMovement attemptedMove)
    {
        if(attemptedMove.NewZoom > ZoomMax)
        {
            return new CameraMovement(ZoomMax, attemptedMove.Camera);
        }

        if(attemptedMove.NewZoom < ZoomMin)
        {
            return new CameraMovement(ZoomMin, attemptedMove.Camera);
        }

        return attemptedMove;
    }

}