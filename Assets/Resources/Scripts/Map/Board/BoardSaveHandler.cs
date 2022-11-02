using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSaveHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("LoadingMainMenu", SendBoardToSave);    
    }

    void SendBoardToSave(Dictionary<string, object> empty)
    {
        EventManager.TriggerEvent("SaveGame", new Dictionary<string, object> { { "board", gameObject } });
    }

    private void OnDestroy()
    {
        EventManager.StopListening("LoadingMainMenu", SendBoardToSave);
    }

}
