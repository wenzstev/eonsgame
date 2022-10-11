using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Save
{

    public SerializedBoard sboard;

    public Save(Board b)
    {
        sboard = new SerializedBoard(b);
    }

    public static void SerializeSave(Save saveData, string saveName)
    {
        string saveFileLocation = $"{Application.persistentDataPath}/{saveName}.json";
        Debug.Log(saveFileLocation);

        string saveFileJson = JsonUtility.ToJson(saveData);
        Debug.Log(saveFileJson);
        File.WriteAllText(saveFileLocation, saveFileJson);

    }

    public static Save UnserializeSave(string savePath)
    {
        string saveJson = File.ReadAllText(savePath);
        return JsonUtility.FromJson<Save>(saveJson);
    }

    public static GameObject CreatePersistantSave(Save save)
    {
        GameObject saveObj = new GameObject("SaveFile");
        SaveObject so = saveObj.AddComponent<SaveObject>();
        so.InitializeSave(save);
        return saveObj;
    }
}






