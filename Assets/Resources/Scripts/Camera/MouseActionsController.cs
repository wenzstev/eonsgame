using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseActionsController : MonoBehaviour
{

    bool isMouseMovingCamera = false;

    private void Start()
    {
        EventManager.StartListening("MouseUpTile", DetermineIfTileCanInteract); // TODO: change this so that the "MouseUp" event is triggered generically for items in the scene
        EventManager.StartListening("MouseDragInAction", SetMouseDragToTrue);
        EventManager.StartListening("MouseDragStopped", SetMouseDragToFalse);
    }

    void Update()
    {
        if(Input.GetMouseButtonUp(0) && !isMouseMovingCamera) // TODO: move all mouse button commands into a single input class
        {
            Debug.Log("MouseUpGeneric");
            EventManager.TriggerEvent("MouseUpGeneric", new Dictionary<string, object>() { { "button", 0 } });
        }
    }

    void DetermineIfTileCanInteract(Dictionary<string, object> tileInfo)
    { 
        if(!isMouseMovingCamera)
        {
            Debug.Log("InteractiveMouseUp");
            EventManager.TriggerEvent("InteractiveMouseUp", tileInfo);
        }
    }

    void SetMouseDragToTrue(Dictionary<string, object> empty)
    {
        isMouseMovingCamera = true;
    }

    void SetMouseDragToFalse (Dictionary<string, object> empty)
    {
        isMouseMovingCamera = false;
    }

    private void OnDestroy()
    {
        EventManager.StopListening("MouseUpTile", DetermineIfTileCanInteract);
        EventManager.StopListening("MouseDragInAction", SetMouseDragToTrue);
        EventManager.StopListening("MouseDragStopped", SetMouseDragToFalse);
    }

}
