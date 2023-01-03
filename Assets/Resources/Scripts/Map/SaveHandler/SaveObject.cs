using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene()); // undo DontDestroyOnLoad so that the object is destroyed if we return to the main menu

        return Save.UnserializeSave(saveName);

    }

    void SaveGame(Dictionary<string, object> payLoad)
    {
        Debug.Log("saving game");
        GameObject boardToSave = (GameObject)payLoad["board"];

        Save newSave = new Save(boardToSave.GetComponent<Board>());
        Save.SerializeSave(newSave, saveName);
    }

   

}
