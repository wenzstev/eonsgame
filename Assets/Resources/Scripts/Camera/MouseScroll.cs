using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScroll : MonoBehaviour
{
    public float scale;
    public CameraMoveController CameraMoveController;

    void Update()
    {
        float newScale = Input.mouseScrollDelta.y * scale;

        if(newScale != 0)
        {
            CameraMovement move = new CameraMovement(newScale + Camera.main.orthographicSize, Camera.main);
            CameraMoveController.AttemptMove(move);
        }
    }

}
