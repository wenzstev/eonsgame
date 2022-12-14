using System.Collections;
using UnityEngine;


public class CameraBounds : MonoBehaviour
{

    private void Awake()
    {
        AddColliderOnCamera();
    }


    public void AddColliderOnCamera()
    {
        if(Camera.main == null)
        {
            Debug.LogError("No camera found. Make sure you have tagged your main camera!");
            return;
        }

        Camera cam = Camera.main;
        if(!cam.orthographic)
        {
            Debug.LogError("Make sure your camera is set to orthographic mode!");
            return;
        }

        EdgeCollider2D edgeCollider = gameObject.GetComponent<EdgeCollider2D>();

        Vector2 leftBottom = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 leftTop = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        Vector2 rightBottom = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));
        Vector2 rightTop = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));

        Vector2[] edgePoints = new Vector2[] { leftBottom, leftTop, rightTop, rightBottom };

        edgeCollider.points = edgePoints;
    }

}
