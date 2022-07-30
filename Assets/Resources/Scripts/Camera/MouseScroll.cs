using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScroll : MonoBehaviour
{
    public float scale;

    void Update()
    {
        Camera.main.orthographicSize += Input.mouseScrollDelta.y * scale;
        if(Camera.main.orthographicSize < 1)
        {
            Camera.main.orthographicSize = 1;
        }
    }
}
