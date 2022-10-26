using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    public string saveName;

    private void Start()
    {
        EventManager.StartListening("SaveGame", SaveGame);
    }

    public void InitializeSave(string s)
    {
        saveName = s;
        DontDestroyOnLoad(gameObject);
    }

    public Save LoadPersistantSave()
    {
        return Save.UnserializeSave(saveName);
    }

    void SaveGame(Dictionary<string, object> payLoad)
    {
        GameObject boardToSave = (GameObject)payLoad["board"];

        Save newSave = new Save(boardToSave.GetComponent<Board>());
        Save.SerializeSave(newSave, saveName);
    }

   

}
