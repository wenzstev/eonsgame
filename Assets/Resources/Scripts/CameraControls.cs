using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    public float speed;
    public float zoomspeed;

    public float minZoom;
    public float maxZoom;

    new Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x-speed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
        if(Input.GetKey(KeyCode.Q) && camera.orthographicSize >= minZoom)
        {
            camera.orthographicSize = camera.orthographicSize - zoomspeed;
        }
        if (Input.GetKey(KeyCode.E) && camera.orthographicSize <= maxZoom)
        {
            camera.orthographicSize = camera.orthographicSize + zoomspeed;
        }
    }
}
