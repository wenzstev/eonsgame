using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ZoomToObj : MonoBehaviour
{
    GameObject SelectedObj;

    public float zoomSize = 4;

    public void SetSelectedObj(GameObject sel)
    {
        SelectedObj = sel;
    }

    public void ZoomToSelectedTile()
    {
        CameraMoveController cmc = Camera.main.GetComponent<CameraMoveController>();

        cmc.AttemptMove(new CameraMovement(zoomSize, SelectedObj.transform.position, Camera.main));
    }

}
