using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    Vector3 mouseOriginPosition;
    Vector3 cameraOriginPosition;

    public float speedModifierX = 2;
    public float speedModifierY = 2;

    bool isDragging;

    public CameraMoveController CameraMoveController;


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseOriginPosition = Input.mousePosition;
            cameraOriginPosition = transform.position;
        }

        if (!Input.GetMouseButton(0))
        {
            if(isDragging)
            {
                isDragging = false;
                EventManager.TriggerEvent("MouseDragStopped", null);
            }
            return;
        };

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOriginPosition);
        
        if(!isDragging && pos.magnitude > 0)
        {
            isDragging = true;
            EventManager.TriggerEvent("MouseDragInAction", null);
        }

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
