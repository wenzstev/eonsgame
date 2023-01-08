using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class MouseActionsController : MonoBehaviour
{
    public event EventHandler<MouseActionEventArgs> MouseDownAction;
    public event EventHandler<MouseActionEventArgs> MouseUpAction;
    public event EventHandler<MouseActionEventArgs> MouseDragInAction;
    public event EventHandler<MouseActionEventArgs> MouseDragStopped;

    bool isDragging;


    Vector3 mouseOriginPosition = Vector3.left; // value to show there is not a current drag


    void Update()
    {
        /*
         
         if left mouse down
            if not over ui:
                fire regular mouse event

        if left mouse up
            if drag was not in action:
                fire mouse up event


        fire regular mouse event:
            
            raycast point that mouse was clicked on, going down
            select first item clicked that has a collider
            inform item that it was clicked
            fire global clicked event


         
         */

        if(Input.GetMouseButtonDown(0) && !IsMouseOverUI())
        {
            MouseDownAction?.Invoke(this, new MouseActionEventArgs());
            mouseOriginPosition = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            mouseOriginPosition = Vector3.left;
            if (isDragging)
            {
                StopDragging();
            }
            else if(!IsMouseOverUI()) MouseUpAction?.Invoke(this, new MouseActionEventArgs());
        }

        if(Input.GetMouseButton(0) && mouseOriginPosition != Vector3.left) // we use "left" to indicate that the mouse is held over a UI element
        {
            if (Input.mousePosition != mouseOriginPosition)
            {
                isDragging = true;
                MouseDragInAction?.Invoke(this, new MouseActionEventArgs());
            }
        }
    }

    void StopDragging()
    {
        MouseDragStopped?.Invoke(this, new MouseActionEventArgs());
        isDragging = false;
    }

    bool IsMouseOverUI()
    {
        Debug.Log(EventSystem.current.IsPointerOverGameObject());
        return EventSystem.current.IsPointerOverGameObject();
    }


    public class MouseActionEventArgs : EventArgs
    {
        public Vector3 MousePosition;
        public GameObject[] ClickedObjects;

        public MouseActionEventArgs()
        {
            MousePosition = Input.mousePosition;
            ClickedObjects = GetHitObjects(MousePosition);
        }

        // check if the mouse position is over a collider
        static GameObject[] GetHitObjects(Vector2 origin)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(origin);
            RaycastHit2D[] results = Physics2D.RaycastAll(worldPoint, Vector2.up, 0); // just the point of being clicked
            return results.Select(r => r.collider.gameObject).ToArray();

        }

        public GameObject GetFirstThatContains<T>()
        {
            foreach(GameObject go in ClickedObjects)
            {
                if (go.GetComponent<T>() != null) return go;
            }

            return null;
        }
    }

}
