using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMapButton : MonoBehaviour
{
    Button saveButton;

    // Start is called before the first frame update
    void Start()
    {
        saveButton = GetComponent<Button>();
        EventManager.StartListening("MapDraftCreated", OnMapDraftCreated);
    }

    
    void OnMapDraftCreated(Dictionary<string, object> empty)
    {
        saveButton.interactable = true;
    }


    public void SaveMap()
    {
        EventManager.TriggerEvent("SaveMap", null);
    }
}
