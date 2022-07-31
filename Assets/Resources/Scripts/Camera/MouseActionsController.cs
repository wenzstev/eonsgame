using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseActionsController : MonoBehaviour
{

    bool isMouseMovingCamera = false;

    private void Start()
    {
        EventManager.StartListening("MouseUpTile", DetermineIfTileCanInteract);
        EventManager.StartListening("MouseDragInAction", SetMouseDragToTrue);
        EventManager.StartListening("MouseDragStopped", SetMouseDragToFalse);
    }

    void DetermineIfTileCanInteract(Dictionary<string, object> tileInfo)
    { 
        if(!isMouseMovingCamera)
        {
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
