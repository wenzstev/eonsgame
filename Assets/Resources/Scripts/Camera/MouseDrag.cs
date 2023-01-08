using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    Vector3 mouseOriginPosition;
    Vector3 cameraOriginPosition;



    public float speedModifierX = 2;
    public float speedModifierY = 2;


    public CameraMoveController CameraMoveController;
    public MouseActionsController MouseActionsController;


    private void Awake()
    {
        MouseActionsController.MouseDownAction += MouseDrag_OnMouseDownAction;
        MouseActionsController.MouseDragInAction += MouseDrag_OnMouseDragInAction;
    }

    void MouseDrag_OnMouseDownAction(object sender, MouseActionsController.MouseActionEventArgs e)
    {
        cameraOriginPosition = transform.position;
        mouseOriginPosition = e.MousePosition;
    }

    void MouseDrag_OnMouseDragInAction(object sender, MouseActionsController.MouseActionEventArgs e)
    {
        Vector3 pos = Camera.main.ScreenToViewportPoint(e.MousePosition - mouseOriginPosition);
        AdjustCameraLocation(pos);
    }

    public void AdjustCameraLocation(Vector3 pos)
    {
        float size = Camera.main.orthographicSize;
        Vector3 adjustedPos = Vector3.Scale(pos, new Vector3(size * speedModifierX, size * speedModifierY, 1));
        Vector3 newPos = cameraOriginPosition - adjustedPos;

        CameraMovement move = new CameraMovement(newPos, Camera.main);
        CameraMoveController.AttemptMove(move);
    }

}
