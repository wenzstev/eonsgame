using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveNamedMap : MonoBehaviour
{
    public TMP_InputField input;

    private void Start()
    {
        EventManager.StartListening("BoardProvided", OnBoardProvided);
    }
    public void OnSaveMapClicked()
    {
        EventManager.TriggerEvent("BoardRequested", null);
    }

    void OnBoardProvided(Dictionary<string, object> payload)
    {
        string saveName = input.text;

        Board b = (Board)payload["board"];

        Save save = new Save(b);
        Save.SerializeSave(save, saveName);
    }
}
